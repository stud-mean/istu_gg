using System;

namespace LightControlSystem
{
    // Интерфейс для устройства освещения
    public interface ILightDevice
    {
        string Id { get; }
        string Name { get; set; }
        bool IsOn { get; }
        int Brightness { get; set; }
        ColorTemperature ColorTemp { get; set; }

        void TurnOn();
        void TurnOff();
        void Toggle();
        string GetStatus();
    }

    // Перечисление для цветовой температуры
    public enum ColorTemperature
    {
        Warm = 2700,
        Neutral = 4000,
        Cool = 5000,
        Daylight = 6500
    }
}