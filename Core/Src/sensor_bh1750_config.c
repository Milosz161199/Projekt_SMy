/**
  ******************************************************************************
  * @file    sensor_bh1750_config.c
  * @author  Milosz Plutowski
  * @version V1.0
  * @date    21-Jan-2021
  * @brief   Simple BH1750 driver library for STM32F7 configuration file.
  *
  ******************************************************************************
  */

/* Include  ---------------------------------------------------------------------------*/
#include "sensor_bh1750_config.h"
#include "sensor_bh1750.h"
#include "main.h"
#include "i2c.h"


/* Public Variables ------------------------------------------------------------------*/
BH1750_HandleTypeDef hbh1750_1 = {
		.I2C = &hi2c1, .Address = BH1750_ADDRESS_L, .Timeout = 0x0064
};
