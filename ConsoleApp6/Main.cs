using System;
using System.Collections.Generic;

namespace LightControlSystem
{
    // Основная программа
    class Program
    {
        static LightControlSystem lightSystem = new LightControlSystem();

        static void Main()
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ОСВЕЩЕНИЕМ ===\n");

            // Добавляем устройства
            InitializeDevices();

            bool exit = false;

            while (!exit)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        lightSystem.ShowStatus();
                        break;
                    case "2":
                        ControlDevice();
                        break;
                    case "3":
                        lightSystem.TurnOnAll();
                        break;
                    case "4":
                        lightSystem.TurnOffAll();
                        break;
                    case "5":
                        SetBrightnessForAll();
                        break;
                    case "6":
                        ShowDeviceStats();
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Выход из программы");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=== МЕНЮ УПРАВЛЕНИЯ ===");
            Console.WriteLine("1. Показать статус всех устройств");
            Console.WriteLine("2. Управление устройством");
            Console.WriteLine("3. Включить все устройства");
            Console.WriteLine("4. Выключить все устройства");
            Console.WriteLine("5. Установить яркость для всех");
            Console.WriteLine("6. Статистика системы");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите действие: ");
        }

        static void InitializeDevices()
        {
            lightSystem.AddDevice(new SmartBulb("bulb1", "Люстра в гостиной"));
            lightSystem.AddDevice(new SmartBulb("bulb2", "Торшер"));
            lightSystem.AddDevice(new SmartBulb("bulb3", "Свет в спальне"));
            lightSystem.AddDevice(new LEDStrip("strip1", "LED лента на кухне"));
            lightSystem.AddDevice(new LEDStrip("strip2", "Подсветка ТВ"));
        }

        static void ControlDevice()
        {
            Console.WriteLine("\n=== УПРАВЛЕНИЕ УСТРОЙСТВОМ ===");

            if (lightSystem.DeviceCount == 0)
            {
                Console.WriteLine("Нет доступных устройств");
                return;
            }

            // Показываем устройства с номерами
            int index = 1;
            var devicesList = new List<ILightDevice>(lightSystem.GetAllDevices());
            foreach (var device in devicesList)
            {
                Console.WriteLine($"{index}. {device.GetStatus()}");
                index++;
            }

            Console.Write("\nВыберите номер устройства для управления (1-5): ");
            if (int.TryParse(Console.ReadLine(), out int deviceIndex) &&
                deviceIndex >= 1 && deviceIndex <= devicesList.Count)
            {
                var device = devicesList[deviceIndex - 1];

                Console.WriteLine($"\nУправление: {device.Name}");
                Console.WriteLine($"Текущий статус: {device.GetStatus()}");
                Console.WriteLine("1. Включить");
                Console.WriteLine("2. Выключить");
                Console.WriteLine("3. Переключить (ВКЛ/ВЫКЛ)");
                Console.WriteLine("4. Установить яркость");
                Console.WriteLine("5. Изменить цветовую температуру");

                if (device is LEDStrip)
                {
                    Console.WriteLine("6. Установить цвет RGB");
                }

                Console.Write("Выберите действие: ");
                var action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        device.TurnOn();
                        break;
                    case "2":
                        device.TurnOff();
                        break;
                    case "3":
                        device.Toggle();
                        break;
                    case "4":
                        SetDeviceBrightness(device);
                        break;
                    case "5":
                        SetColorTemperature(device);
                        break;
                    case "6":
                        if (device is LEDStrip ledStrip)
                        {
                            SetLEDColor(ledStrip);
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неверный номер устройства");
            }
        }

        static void SetDeviceBrightness(ILightDevice device)
        {
            Console.Write("Введите яркость (0-100): ");
            if (int.TryParse(Console.ReadLine(), out int brightness))
            {
                device.Brightness = Math.Clamp(brightness, 0, 100);
                Console.WriteLine($"Яркость {device.Name} установлена: {device.Brightness}%");
            }
        }

        static void SetColorTemperature(ILightDevice device)
        {
            Console.WriteLine("\nВыберите цветовую температуру:");
            Console.WriteLine("1. Теплый (2700K) - уютный желтый свет");
            Console.WriteLine("2. Нейтральный (4000K) - естественный белый свет");
            Console.WriteLine("3. Холодный (5000K) - яркий белый свет");
            Console.WriteLine("4. Дневной (6500K) - холодный голубоватый свет");
            Console.Write("Ваш выбор: ");

            var choice = Console.ReadLine();
            device.ColorTemp = choice switch
            {
                "1" => ColorTemperature.Warm,
                "2" => ColorTemperature.Neutral,
                "3" => ColorTemperature.Cool,
                "4" => ColorTemperature.Daylight,
                _ => device.ColorTemp
            };

            Console.WriteLine($"Установлена цветовая температура: {device.ColorTemp}");
        }

        static void SetLEDColor(LEDStrip ledStrip)
        {
            Console.WriteLine("\nДоступные цвета:");
            Console.WriteLine("1. Красный (#FF0000)");
            Console.WriteLine("2. Зеленый (#00FF00)");
            Console.WriteLine("3. Синий (#0000FF)");
            Console.WriteLine("4. Фиолетовый (#FF00FF)");
            Console.WriteLine("5. Голубой (#00FFFF)");
            Console.WriteLine("6. Желтый (#FFFF00)");
            Console.WriteLine("7. Белый (#FFFFFF)");
            Console.Write("Выберите цвет (или введите свой в формате #RRGGBB): ");

            var input = Console.ReadLine();
            string color = input switch
            {
                "1" => "#FF0000",
                "2" => "#00FF00",
                "3" => "#0000FF",
                "4" => "#FF00FF",
                "5" => "#00FFFF",
                "6" => "#FFFF00",
                "7" => "#FFFFFF",
                _ => input.StartsWith("#") && input.Length == 7 ? input : "#FFFFFF"
            };

            ledStrip.SetColor(color);
        }

        static void SetBrightnessForAll()
        {
            Console.Write("Введите яркость для всех устройств (0-100): ");
            if (int.TryParse(Console.ReadLine(), out int brightness))
            {
                lightSystem.SetBrightnessForAll(brightness);
            }
        }

        static void ShowDeviceStats()
        {
            Console.WriteLine("\n=== СТАТИСТИКА СИСТЕМЫ ===");
            Console.WriteLine($"Всего устройств: {lightSystem.DeviceCount}");

            int onCount = 0;
            int totalBrightness = 0;

            foreach (var device in lightSystem.GetAllDevices())
            {
                if (device.IsOn) onCount++;
                totalBrightness += device.Brightness;
            }

            Console.WriteLine($"Включено устройств: {onCount}");
            Console.WriteLine($"Выключено устройств: {lightSystem.DeviceCount - onCount}");

            if (lightSystem.DeviceCount > 0)
            {
                Console.WriteLine($"Средняя яркость: {totalBrightness / lightSystem.DeviceCount}%");
            }
        }
    }
}