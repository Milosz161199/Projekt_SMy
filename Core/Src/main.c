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
#include "i2c.h"
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
#define enc_counter_min     2
#define enc_counter_step    1

// size of message
#define SP_MSG_SIZE 3

/* Choose PID parameters */
#define PID_PARAM_KP        3.0f              /* Proporcional 5.0 */ //6.0
#define PID_PARAM_KI        1.5f              /* Integral 0.4 */ // 1.8
#define PID_PARAM_KD        0.0f              /* Derivative */ // 0.1
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/

/* USER CODE BEGIN PV */

uint8_t enc_counter_max = 110;

// Instance of PID
arm_pid_instance_f32 pid;

uint32_t set_point = 0.0f;
uint32_t LED_lux_int = 0;
uint32_t enc_counter = enc_counter_min;

float32_t LED_lux = 0.0f;
float32_t PID_out = 0.0f;
float32_t PID_error = 0.0f;
float32_t PID_error_in_procent = 0.0f;
float32_t d_PWM = 0.0f;

bool BTN_State_1 = FALSE;      // toggle to switch number of LEDs
bool LCD_show_ERROR = FALSE;   // toggle to switch screens on lcd

// USART3
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
		// reading from the sensor and writing the value to the variable LED_lux
		LED_lux = SENSOR_BH1750_ReadLux(&hbh1750_1);
		// convert float32_t to uint32_t
		LED_lux_int = (uint32_t)LED_lux;
		tx_n = sprintf(tx_buffer, "%03x", LED_lux_int);
		// send a message if the condition is true
		if(tx_n == SP_MSG_SIZE )
			 HAL_UART_Transmit(&huart3, (uint8_t*)tx_buffer, tx_n, 1);


		/* CONTROL PROCESS PID */
		PID_error = set_point - LED_lux;
		PID_error_in_procent = (fabs(PID_error) / (enc_counter_max - enc_counter_min)) * 100;
		PID_out = arm_pid_f32(&pid, PID_error);


		// Additional visualization of the correctness of the regulation system
		if(PID_error_in_procent <= 1.0 )
		{
			// if correct turn green LED (LD1) on and turn red LED (LD3) off
			HAL_GPIO_WritePin(LD1_GPIO_Port, LD1_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(LD3_GPIO_Port, LD3_Pin, GPIO_PIN_RESET);
		}
		else
		{
			// if not correct turn green LED (LD1) off and turn red LED (LD3) on
			HAL_GPIO_WritePin(LD3_GPIO_Port, LD3_Pin, GPIO_PIN_SET);
			HAL_GPIO_WritePin(LD1_GPIO_Port, LD1_Pin, GPIO_PIN_RESET);
		}

		// Output saturation
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

		// PWM filling control of diodes
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
	if(GPIO_Pin == ENC_CLK_Pin) // Rotary pulser for setting the value of control 'set_point'
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
	else if(GPIO_Pin == EX1_Btn_Pin) // button for switching the number of LEDs
	{
		BTN_State_1 = !BTN_State_1;
		if(BTN_State_1 == TRUE)
		{
			enc_counter_max = 180;
		}
		else
		{
			enc_counter_max = 110;
			if(set_point > enc_counter_max)
			{
				set_point = enc_counter_max;
			}
		}
	}
	else if(GPIO_Pin == EX2_Btn_Pin)  // button to switch the lcd screen
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
		if( rx_n == 1){
			set_point = (uint32_t)UART_set_point;
			enc_counter = (uint32_t)UART_set_point;
		}
		else if( RX_DATA[0] == 'T' && RX_DATA[1] == 'W' && RX_DATA[2] == 'O' ){
			BTN_State_1 = TRUE;
			enc_counter_max = 180;
		}
		else if( RX_DATA[0] == 'O' && RX_DATA[1] == 'N' && RX_DATA[2] == 'E' ){
			BTN_State_1 = FALSE;
			enc_counter_max = 110;
			if(set_point > enc_counter_max)
			{
				set_point = enc_counter_max;
			}
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
  MX_USART3_UART_Init();
  MX_TIM2_Init();
  MX_TIM3_Init();
  MX_TIM4_Init();
  MX_I2C1_Init();
  MX_TIM5_Init();
  /* USER CODE BEGIN 2 */

  // Sensor init
  SENSOR_BH1750_Init(&hbh1750_1);

  // Starting values
  LED_lux = 2.0f;
  set_point = 2.0;

  /****  PID  ****/
  // Set the PID parameters
  pid.Kp = PID_PARAM_KP;
  pid.Ki = PID_PARAM_KI;
  pid.Kd = PID_PARAM_KD;
  // Init PID with constant period
  arm_pid_init_f32(&pid, 1);
  __HAL_TIM_SET_AUTORELOAD(&htim2, 129999);
  HAL_TIM_Base_Start_IT(&htim2);


  // Init PWM channels of TIM3
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_1);
  HAL_TIM_PWM_Start(&htim3, TIM_CHANNEL_2);

  // Init lcd
  LCD_Init(&hlcd1);

  // Data display on lcd every 10ms
  //__HAL_TIM_SET_AUTORELOAD(&htim7, 9999);
  //HAL_TIM_Base_Start_IT(&htim7);


  // USART3
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
