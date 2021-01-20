/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "adc.h"
#include "dac.h"
#include "dma.h"
#include "i2c.h"
#include "spi.h"
#include "tim.h"
#include "usart.h"
#include "gpio.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "arm_math.h"
#include "math.h"
#include "sensor_bh1750.h"
#include "sensor_bh1750_config.h"
#include "stdio.h"
#include "lcd.h"
#include "lcd_config.h"
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */
typedef enum {FALSE = 0, TRUE} bool;
/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */
uint8_t enc_counter_max = 125;
#define enc_counter_min  0
#define enc_counter_step  1

#define SP_MSG_SIZE 3

/* Choose PID parameters */
#define PID_PARAM_KP        5.0f              /* Proporcional 5.0 */
#define PID_PARAM_KI        3.0f              /* Integral 0.4 */
#define PID_PARAM_KD        0.0f              /* Derivative */
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/
/* USER CODE BEGIN PV */

arm_pid_instance_f32 pid;

float32_t LED_lux = 0.0f;
uint32_t set_point = 0.0f;
float32_t PID_out = 0.0f;
float32_t PID_error = 0.0f;
float32_t PID_error_in_procent = 0.0f;
float32_t d_PWM = 0.0f;
uint32_t LED_lux_int = 0;

bool BTN_State_1 = FALSE;
bool LCD_show_ERROR = FALSE;
uint32_t enc_counter = enc_counter_min;

// Komunikacja USART3
uint8_t tx_n;
uint8_t tx_buffer[50];
uint8_t RX_DATA[SP_MSG_SIZE + 1];

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */
void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef *htim)
{
	if(htim->Instance == TIM2)
	{
		// Odczyt sensora i zapis odczytanej wartości do zmiennej LED_lux
		LED_lux = SENSOR_BH1750_ReadLux(&hbh1750_1);
		// konwersja float32_t na uint32_t
		LED_lux_int = (uint32_t)LED_lux;
		tx_n = sprintf(tx_buffer, "%03x", LED_lux_int);
		// wysłanie wiadomości pod określonym warunkiem
		if(tx_n == SP_MSG_SIZE )
			 HAL_UART_Transmit(&huart3, (uint8_t*)tx_buffer, tx_n, 1);

		// Ustawianie parametrów regulatora PID
		pid.Kp = PID_PARAM_KP;
		pid.Ki = PID_PARAM_KI;
		pid.Kd = PID_PARAM_KD;

		// wejście regulatora PID
		PID_error = set_point - LED_lux;
		PID_error_in_procent = (fabs(PID_error)/(enc_counter_max - enc_counter_min))*100;
		PID_out = arm_pid_f32(&pid, PID_error);


		// Dodatkowa sygnalizacja poprawności układu regulacji
		if(PID_error_in_procent <= 1.0 )
		{
			// zakończy powodzeniem załącz diodę zieloną
			HAL_GPIO_WritePin(LD1_GPIO_Port, LD1_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(LD3_GPIO_Port, LD3_Pin, GPIO_PIN_RESET);
		}
		else
		{
			// w innym przypadku załącz diodę czerwoną
			HAL_GPIO_WritePin(LD3_GPIO_Port, LD3_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(LD1_GPIO_Port, LD1_Pin, GPIO_PIN_RESET);
		}


		if(PID_out > 1000)
		{
			d_PWM = 1000;
		}
		else if(PID_out < 0)
		{
			d_PWM = 0;
		}
		else
		{
			d_PWM = PID_out;
		}

		// sterowanie wypełnieniem diod LED
		if(BTN_State_1)
		{
			__HAL_TIM_SET_COMPARE(&htim3, TIM_CHANNEL_1, d_PWM);
			__HAL_TIM_SET_COMPARE(&htim3, TIM_CHANNEL_2, d_PWM);
		}
		else
		{
			__HAL_TIM_SET_COMPARE(&htim3, TIM_CHANNEL_1, 0);
			__HAL_TIM_SET_COMPARE(&htim3, TIM_CHANNEL_2, d_PWM);
		}
	}
}

void HAL_GPIO_EXTI_Callback(uint16_t GPIO_Pin)
{
	if(GPIO_Pin == ENC_CLK_Pin) // Impulsator obrotowy
	{
		 if (HAL_GPIO_ReadPin(ENC_DT_GPIO_Port, ENC_DT_Pin) == GPIO_PIN_RESET)
		 {
			 enc_counter = (enc_counter >= enc_counter_max) ? enc_counter_max :	(enc_counter + enc_counter_step);
			 set_point = enc_counter;
		 }
		 else
		 {
			 enc_counter = (enc_counter <= enc_counter_min) ? enc_counter_min :	(enc_counter - enc_counter_step);
			 set_point = enc_counter;
		 }
	}
	else if(GPIO_Pin == EX1_Btn_Pin) // przycisk, do przełączania liczby sterowanych diod
	{
		BTN_State_1 = !BTN_State_1;
		if(BTN_State_1 == TRUE)
		{
			enc_counter_max = 180.0f;

		}
		else
		{
			enc_counter_max = 125.0f;
			if(set_point > enc_counter_max)
			{
				set_point = enc_counter_max;
			}
		}
	}
	else if(GPIO_Pin == EX2_Btn_Pin)  // przycisk, do przełączania ekranu w lcd
	{
		LCD_show_ERROR = !LCD_show_ERROR;
	}
}

void HAL_UART_RxCpltCallback( UART_HandleTypeDef *huart )
{
	if( huart->Instance == USART3 )
	{
		/* Set point value read from serial port . */
		static int UART_set_point;
		/* Number of items successfully filled by 'sscanf '. */
		static int rx_n;
		/* Three - digit hexadecimal number : C- string to integer . */
		rx_n = sscanf((char*)RX_DATA, "%3x", &UART_set_point );
		/* If conversion if successful set set_point value or set control by one or two LEDs . */
		if( rx_n == 1)
			set_point = (uint32_t)UART_set_point;
		else if( RX_DATA[0] == 'T' && RX_DATA[1] == 'W' && RX_DATA[2] == 'O' ){
			BTN_State_1 = TRUE;
		}
		else if( RX_DATA[0] == 'O' && RX_DATA[1] == 'N' && RX_DATA[2] == 'E' ){
			BTN_State_1 = FALSE;
		}
		/* Start listening for the next message . */
		HAL_UART_Receive_IT(&huart3, RX_DATA, SP_MSG_SIZE);
	}
}
/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
  /* USER CODE BEGIN 1 */

  /* USER CODE END 1 */

  /* MCU Configuration--------------------------------------------------------*/

  /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
  HAL_Init();

  /* USER CODE BEGIN Init */

  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  /* USER CODE BEGIN SysInit */

  /* USER CODE END SysInit */

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_DMA_Init();
  MX_USART3_UART_Init();
  MX_TIM2_Init();
  MX_TIM3_Init();
  MX_TIM4_Init();
  MX_I2C1_Init();
  MX_SPI4_Init();
  MX_ADC1_Init();
  MX_DAC_Init();
  MX_TIM6_Init();
  MX_TIM7_Init();
  MX_TIM5_Init();
  /* USER CODE BEGIN 2 */

  // Inicjalizacja sensora
  SENSOR_BH1750_Init(&hbh1750_1);

  // Obsługa regulatora PID
  arm_pid_init_f32(&pid, 1);
  HAL_TIM_Base_Start_IT(&htim2);
  __HAL_TIM_SET_AUTORELOAD(&htim2, 129999);

  // Inicjalizajca kanałów PWM timera TIM3
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_1);
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_2);

  // Inicjalizacja lcd
  LCD_Init(&hlcd1);
  // Wyświetlanie danych na Lcd co 10ms
  //HAL_TIM_Base_Start_IT(&htim7);
  //__HAL_TIM_SET_AUTORELOAD(&htim7, 9999);

  // Port szeregowy USART3
  HAL_UART_Receive_IT(&huart3, RX_DATA, SP_MSG_SIZE);

  /* USER CODE END 2 */

  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
    /* USER CODE END WHILE */
	  if(LCD_show_ERROR)
	  		{
	  			LCD_SetCursor(&hlcd1, 0, 0);
	  			LCD_printf(&hlcd1, "ERROR: %5.2f [ ]", PID_error_in_procent);
	  		  	// LCD_SetCursor(&hlcd1, 0, 14);
	  			// lcd_write(&hlcd1, &data, 8);
	  		 	LCD_SetCursor(&hlcd1, 1, 0);
	  		 	LCD_printStr(&hlcd1, "               ");
	  		}
	  		else
	  		{
	  			LCD_SetCursor(&hlcd1, 0, 0);
	  			LCD_printf(&hlcd1, "WYJ: %5.2f [lux] ", (float32_t)LED_lux);
	  			LCD_SetCursor(&hlcd1, 1, 0);
	  			LCD_printf(&hlcd1, "ZAD: %03d [lux]  ", (uint32_t)set_point);
	  		}
	  HAL_Delay(10);
    /* USER CODE BEGIN 3 */
  }
  /* USER CODE END 3 */
}

/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};
  RCC_PeriphCLKInitTypeDef PeriphClkInitStruct = {0};

  /** Configure LSE Drive Capability
  */
  HAL_PWR_EnableBkUpAccess();
  /** Configure the main internal regulator output voltage
  */
  __HAL_RCC_PWR_CLK_ENABLE();
  __HAL_PWR_VOLTAGESCALING_CONFIG(PWR_REGULATOR_VOLTAGE_SCALE1);
  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
  RCC_OscInitStruct.HSEState = RCC_HSE_BYPASS;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
  RCC_OscInitStruct.PLL.PLLM = 4;
  RCC_OscInitStruct.PLL.PLLN = 216;
  RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
  RCC_OscInitStruct.PLL.PLLQ = 3;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }
  /** Activate the Over-Drive mode
  */
  if (HAL_PWREx_EnableOverDrive() != HAL_OK)
  {
    Error_Handler();
  }
  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV4;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV2;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_7) != HAL_OK)
  {
    Error_Handler();
  }
  PeriphClkInitStruct.PeriphClockSelection = RCC_PERIPHCLK_USART3|RCC_PERIPHCLK_I2C1;
  PeriphClkInitStruct.Usart3ClockSelection = RCC_USART3CLKSOURCE_PCLK1;
  PeriphClkInitStruct.I2c1ClockSelection = RCC_I2C1CLKSOURCE_PCLK1;
  if (HAL_RCCEx_PeriphCLKConfig(&PeriphClkInitStruct) != HAL_OK)
  {
    Error_Handler();
  }
}

/* USER CODE BEGIN 4 */

/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
