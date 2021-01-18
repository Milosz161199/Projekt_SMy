/* Include */
#include "sensor_bh1750.h"


/* Public function definition */
void SENSOR_BH1750_Init(BH1750_HandleTypeDef * hbh1750)
{
	uint8_t command;

	command = BH1750_POWER_ON;
	HAL_I2C_Master_Transmit(hbh1750->I2C, hbh1750->Address, &command, 1 ,hbh1750->Timeout);

	command = BH1750_CONTINOUS_H_RES_MODE;
	HAL_I2C_Master_Transmit(hbh1750->I2C, hbh1750->Address, &command, 1 ,hbh1750->Timeout);
}

float SENSOR_BH1750_ReadLux(BH1750_HandleTypeDef * hbh1750)
{
	float light_in_lux = 0.0;
	uint8_t buffer[2];

	//odczyt z czujnika i zapisanie danych w buforze
	HAL_I2C_Master_Receive(hbh1750->I2C, hbh1750->Address, buffer, 2, hbh1750->Timeout);

	light_in_lux = ((buffer[0] << 8) | buffer[1])/1.2;

	//zwaracamy wartość zmiennoprzecinkową wyrażoną w luksach
	return light_in_lux;
}
