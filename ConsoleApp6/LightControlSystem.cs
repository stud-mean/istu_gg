using System;
using System.Collections.Generic;

namespace LightControlSystem
{
    // Система управления освещением
    public class LightControlSystem
    {
        private Dictionary<string, ILightDevice> devices = new Dictionary<string, ILightDevice>();

        public void AddDevice(ILightDevice device)
        {
            devices[device.Id] = device;
        }

        public ILightDevice GetDevice(string deviceId)
        {
            return devices.ContainsKey(deviceId) ? devices[deviceId] : null;
        }

        public void TurnOnAll()
        {
            foreach (var device in devices.Values)
            {
                device.TurnOn();
            }
            Console.WriteLine("Все устройства включены");
        }

        public void TurnOffAll()
        {
            foreach (var device in devices.Values)
            {
                device.TurnOff();
            }
            Console.WriteLine("Все устройства выключены");
        }

        public void SetBrightnessForAll(int brightness)
        {
            foreach (var device in devices.Values)
            {
                device.Brightness = Math.Clamp(brightness, 0, 100);
            }
            Console.WriteLine($"Установлена яркость {brightness}% для всех устройств");
        }

        public void ShowStatus()
        {
            Console.WriteLine("\n=== СТАТУС СИСТЕМЫ ОСВЕЩЕНИЯ ===");
            Console.WriteLine($"Всего устройств: {devices.Count}");

            int index = 1;
            foreach (var device in devices.Values)
            {
                Console.WriteLine($"{index}. {device.GetStatus()}");
                index++;
            }
        }

        public IEnumerable<ILightDevice> GetAllDevices()
        {
            return devices.Values;
        }

        public int DeviceCount => devices.Count;
    }
}