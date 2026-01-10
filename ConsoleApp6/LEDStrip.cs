using System;
using System.Text.RegularExpressions;

namespace LightControlSystem
{
    // LED лента
    public class LEDStrip : LightDevice
    {
        public string Color { get; private set; }

        public LEDStrip(string id, string name) : base(id, name)
        {
            Color = "Белый";
        }

        public void SetColor(string hexColor)
        {
            if (IsValidHexColor(hexColor))
            {
                Color = hexColor.ToUpper();
                Console.WriteLine($"{Name} установлен цвет: {Color}");
            }
            else
            {
                Console.WriteLine($"Ошибка: '{hexColor}' не является допустимым hex-цветом. Используйте формат #RRGGBB.");
            }
        }

        private bool IsValidHexColor(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
                return false;

            var hexPattern = @"^#[0-9A-Fa-f]{6}$";
            return Regex.IsMatch(hexColor, hexPattern);
        }

        public override string GetStatus()
        {
            return base.GetStatus() + $", Цвет RGB: {Color}";
        }
    }
}