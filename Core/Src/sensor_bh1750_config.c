/* Include  */
#include "sensor_bh1750_config.h"
#include "sensor_bh1750.h"
#include "main.h"
#include "i2c.h"


/* Public Variables */
BH1750_HandleTypeDef hbh1750_1 = {
		.I2C = &hi2c1, .Address = BH1750_ADDRESS_L, .Timeout = 0x0064 //Definicje zmiennych struktur dla czujnika_1 0x0064 = 100
};
