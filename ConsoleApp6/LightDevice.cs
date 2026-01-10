using System;

namespace LightControlSystem
{
    // Базовый класс для устройств освещения
    public abstract class LightDevice : ILightDevice
    {
        public string Id { get; }
        public string Name { get; set; }
        public bool IsOn { get; protected set; }
        public int Brightness { get; set; }
        public ColorTemperature ColorTemp { get; set; }

        protected LightDevice(string id, string name)
        {
            Id = id;
            Name = name;
            IsOn = false;
            Brightness = 100;
            ColorTemp = ColorTemperature.Neutral;
        }

        public virtual void TurnOn()
        {
            IsOn = true;
            Console.WriteLine($"{Name} включен");
        }

        public virtual void TurnOff()
        {
            IsOn = false;
            Console.WriteLine($"{Name} выключен");
        }

        public void Toggle()
        {
            if (IsOn)
                TurnOff();
            else
                TurnOn();
        }

        public virtual string GetStatus()
        {
            return $"{Name}: {(IsOn ? "ВКЛ" : "ВЫКЛ")}, Яркость: {Brightness}%, Цвет: {ColorTemp}";
        }
    }
}