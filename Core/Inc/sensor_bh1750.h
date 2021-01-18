/*
 *
 * Sensor BH1750 - odczyt natezenia swiatla za pomoca interfejsu I2C
 * Inicjalizacja/konfiguracja czujnika oraz odczyt pomiaru wywazony w luksach
 *
 * */

#ifndef INC_BH1750_H_
#define INC_BH1750_H_

/* Include  */
#include "stm32f7xx_hal.h"

/* Typedef  */
#define BH1750_I2CType I2C_HandleTypeDef*

/* New Struct Define */
typedef struct{
	BH1750_I2CType I2C;  //interfejs z jaki pracujemy
	uint8_t Address;  //adres
	uint8_t Timeout; //opoznienie
} BH1750_HandleTypeDef; // dedykowany typ zmiennej dla czujnika BH1750

/* Define */
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

/* Public function declaration */
void SENSOR_BH1750_Init(BH1750_HandleTypeDef* hbh1750);  // deklaracja funkcji inicjalizujacej dany sensor
float SENSOR_BH1750_ReadLux (BH1750_HandleTypeDef* hbh1750); // deklaracja funkcji do odczytu pomiaru w luksach


#endif
