﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Hardware;

namespace Sensors.Distance.Vl53l0x_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        VL53L0X _vL53L0X;

        public MeadowApp()
        {
            Initialize();
            Run();

            //InitializeWithShutdownPin();
            //RunWithShutdownPin();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            _vL53L0X = new VL53L0X(i2cBus);
            _vL53L0X.Initialize();
        }

        void InitializeWithShutdownPin()
        {
            Console.WriteLine("Initialize hardware...");
            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            var pin = Device.CreateDigitalOutputPort(Device.Pins.D05, true);
            _vL53L0X = new VL53L0X(i2cBus, pin);
            _vL53L0X.Initialize();
        }

        void Run()
        {
            Console.WriteLine("Run...");

            var range = _vL53L0X.Range();
            Console.WriteLine($"{range} mm");
            
            Thread.Sleep(500);

            _vL53L0X.Units = VL53L0X.UnitType.inches;
            range = _vL53L0X.Range();
            Console.WriteLine($"{range} inches");

            Thread.Sleep(500);

            _vL53L0X.Units = VL53L0X.UnitType.mm;

            for (int i = 0; i < 75; i++)
            {
                Thread.Sleep(200);
                range = _vL53L0X.Range();
                Console.WriteLine($"{range} mm");
            }

            Console.WriteLine("done...");
        }

        void RunWithShutdownPin()
        {
            Console.WriteLine("Run...");

            var range = _vL53L0X.Range();
            Console.WriteLine($"{range} mm");

            Thread.Sleep(500);

            _vL53L0X.Shutdown(true);

            //Range will return -1 since the device is off
            range = _vL53L0X.Range();
            Console.WriteLine($"{range} mm. IsShutdown { _vL53L0X.IsShutdown }");

            //Turn device back on
            _vL53L0X.Shutdown(false);

            range = _vL53L0X.Range();
            Console.WriteLine($"{range} mm. IsShutdown { _vL53L0X.IsShutdown }");

            for (int i = 0; i < 75; i++)
            {
                Thread.Sleep(200);
                range = _vL53L0X.Range();
                Console.WriteLine($"{range} mm");
            }

            Console.WriteLine("done...");
        }

    }
}