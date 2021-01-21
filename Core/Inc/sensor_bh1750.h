/**
  ******************************************************************************
  * @file    sensor_bh1750.h
  * @author  Milosz Plutowski
  * @version V1.0
  * @date    21-Jan-2021
  * @brief   Simple BH1750 driver library for STM32F7.
  *
  ******************************************************************************
  */

#ifndef INC_BH1750_H_
#define INC_BH1750_H_

/* Include -------------------------------------------------------------------*/
#include "stm32f7xx_hal.h"

/* Typedef  */
#define BH1750_I2CType I2C_HandleTypeDef*

/* New Struct Define ---------------------------------------------------------*/
typedef struct{
	BH1750_I2CType I2C;
	uint8_t Address;
	uint8_t Timeout;
} BH1750_HandleTypeDef;

/* Define -------------------------------------------------------------------*/
#define BH1750_ADDRESS_L (0x23 << 1) // ADDR = 'L'
#define BH1750_ADDRESS_H (0x5C << 1) // ADDR = 'H'

#define BH1750_POWER_DOWN               0x00
#define BH1750_POWER_ON                 0x01
#define BH1750_RESET                    0x07
#define BH1750_CONTINOUS_H_RES_MODE     0x10
#define BH1750_CONTINOUS_H_RES_MODE2    0x11
#define BH1750_CONTINOUS_L_RES_MODE     0x13
#define BH1750_ONE_TIME_H_RES_MODE      0x20
#define BH1750_ONE_TIME_H_RES_MODE2     0x21
#define BH1750_ONE_TIME_L_RES_MODE      0x23

/* Public function declaration -----------------------------------------------*/
/*
 * @brief BH1750 initialization procedure.
 * @param[in] hbh1750 BH1750 handler
 * @return None
 */
void SENSOR_BH1750_Init(BH1750_HandleTypeDef* hbh1750);

/*
 * @brief BH1750 measurement reading procedure.
 * @param[in] hbh1750 BH1750 handler
 * @return float light in lux
 */
float SENSOR_BH1750_ReadLux (BH1750_HandleTypeDef* hbh1750);


#endif
