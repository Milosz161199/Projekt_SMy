################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
S_SRCS += \
../Core/Startup/startup_stm32f746zgtx.s 

OBJS += \
./Core/Startup/startup_stm32f746zgtx.o 

S_DEPS += \
./Core/Startup/startup_stm32f746zgtx.d 


# Each subdirectory must supply rules for building sources it contributes
Core/Startup/startup_stm32f746zgtx.o: ../Core/Startup/startup_stm32f746zgtx.s
	arm-none-eabi-gcc -mcpu=cortex-m7 -g3 -c -x assembler-with-cpp -MMD -MP -MF"Core/Startup/startup_stm32f746zgtx.d" -MT"$@" --specs=nano.specs -mfpu=fpv5-sp-d16 -mfloat-abi=hard -mthumb -o "$@" "$<"

