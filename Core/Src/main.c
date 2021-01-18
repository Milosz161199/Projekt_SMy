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
#define enc_counter_max 100
#define enc_counter_min 0
#define enc_counter_step 1
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/

/* USER CODE BEGIN PV */
float32_t SWV_VAR = 0.0f;

float32_t LED_lux = 0.0f;

float32_t set_point = 0.0f;
arm_pid_instance_f32 pid;

float32_t PID_in = 0.0f;
float32_t PID_out = 0.0f;
float32_t PID_error = 0.0f;
float32_t d_PWM = 0.0f;


/* Choose PID parameters */
#define PID_PARAM_KP        5.0f              /* Proporcional */
#define PID_PARAM_KI        3.0f              /* Integral */
#define PID_PARAM_KD        0.0f              /* Derivative */

bool BTN_State_1 = FALSE;
bool LCD_show_ERROR = FALSE;
uint32_t enc_counter = enc_counter_min;


/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */
float32_t RMSE( float32_t* y, float32_t* yref, uint32_t len )
{
	float32_t sum_sq_error = 0;
	for ( uint32_t i = 0; i < len ; i++)
		sum_sq_error += ( yref[i] - y[i] ) * ( yref[i] - y[i] );
	return sqrtf ( sum_sq_error / len );
}


void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef *htim)
{
	if(htim->Instance == TIM2)
	{

		// Odczyt sensora i zapis odczytanej wartości do zmiennej LED_lux
		PID_in = SENSOR_BH1750_ReadLux(&hbh1750_1);


		pid.Kp = PID_PARAM_KP;
		pid.Ki = PID_PARAM_KI;
		pid.Kd = PID_PARAM_KD;

		// wejście regulatora PID
		PID_error = set_point - PID_in;
		PID_out = arm_pid_f32(&pid, PID_error);
        /* Check overflow, duty cycle in percent */

		// Sygnalizacja poprawności układu regulacji
		if(PID_error <= 1.0 )
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


		if(PID_out > 1000){
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
	if(GPIO_Pin == ENC_CLK_Pin)
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
	else if(GPIO_Pin == EX1_Btn_Pin)
	{
		BTN_State_1 = !BTN_State_1;
	}
	else if(GPIO_Pin == EX2_Btn_Pin)
	{
		LCD_show_ERROR = !LCD_show_ERROR;
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
  HAL_TIM_Base_Start_IT(&htim2);
  __HAL_TIM_SET_AUTORELOAD(&htim2, 19999);
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_1);
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_2);
  arm_pid_init_f32(&pid, 1);


  // Wyświetlacz LCD
  LCD_Init(&hlcd1);


  //HAL_ADC_Start(&hadc1);
  //HAL_TIM_Base_Start_IT(&htim6);

  /* USER CODE END 2 */

  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
    /* USER CODE END WHILE */
	  if(LCD_show_ERROR)
	  {
		  LCD_SetCursor(&hlcd1, 0, 0);
		  LCD_printf(&hlcd1, "ERROR: %05d [%]  ", (uint32_t)((PID_error/enc_counter_max)*100));
	  }
	  else
	  {
		  LCD_SetCursor(&hlcd1, 0, 0);
		  LCD_printf(&hlcd1, "WYJ: %03d [lux]  ", (uint32_t)PID_in);
		  LCD_SetCursor(&hlcd1, 1, 0);
		  LCD_printf(&hlcd1, "ZAD: %03d [lux]  ", (uint32_t)set_point);
	  }


	  HAL_Delay(100);
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
