/**
  ******************************************************************************
  * @file    sensor_bh1750.c
  * @author  Milosz Plutowski
  * @version V1.0
  * @date    21-Jan-2021
  * @brief   Simple BH1750 driver library for STM32F7.
  *
  ******************************************************************************
  */

/* Include --------------------------------------------------------------------------*/
#include "sensor_bh1750.h"


/* Public function definition -------------------------------------------------------*/
/*
 * @brief BH1750 initialization procedure.
 * @param[in] hbh1750 BH1750 handler
 * @return None
 */
void SENSOR_BH1750_Init(BH1750_HandleTypeDef * hbh1750)
{
	uint8_t command;

	command = BH1750_POWER_ON;
	HAL_I2C_Master_Transmit(hbh1750->I2C, hbh1750->Address, &command, 1 ,hbh1750->Timeout);

	command = BH1750_CONTINOUS_H_RES_MODE;
	HAL_I2C_Master_Transmit(hbh1750->I2C, hbh1750->Address, &command, 1 ,hbh1750->Timeout);
}

/*
 * @brief BH1750 measurement reading procedure.
 * @param[in] hbh1750 BH1750 handler
 * @return float light in lux
 */
float SENSOR_BH1750_ReadLux(BH1750_HandleTypeDef *hbh1750)
{
	float light_in_lux = 0.0;
	uint8_t buffer[2];

	//reading from the sensor and writing the value to the buffer
	HAL_I2C_Master_Receive(hbh1750->I2C, hbh1750->Address, buffer, 2, hbh1750->Timeout);

	light_in_lux = ((buffer[0] << 8) | buffer[1])/1.2;

	// return float in lux
	return light_in_lux;
}
