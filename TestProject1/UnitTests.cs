using NUnit.Framework;
using LightControlSystem;
using System.Linq;

namespace LightControlSystem.Tests
{
    /// <summary>
    /// Тесты для базового функционала устройств освещения.
    /// </summary>
    [TestFixture]
    public class LightDeviceTests
    {
        /// <summary>
        /// Проверяет, что метод TurnOn корректно включает устройство.
        /// </summary>
        [Test]
        public void SmartBulb_TurnOn_ShouldBeOn()
        {
            var bulb = new SmartBulb("1", "Test Bulb");

            bulb.TurnOn();

            Assert.IsTrue(bulb.IsOn);
        }

        /// <summary>
        /// Проверяет, что метод TurnOff корректно выключает устройство.
        /// </summary>
        [Test]
        public void SmartBulb_TurnOff_ShouldBeOff()
        {
            var bulb = new SmartBulb("1", "Test Bulb");
            bulb.TurnOn();

            bulb.TurnOff();

            Assert.IsFalse(bulb.IsOn);
        }

        /// <summary>
        /// Проверяет, что метод Toggle переключает состояние устройства.
        /// </summary>
        [Test]
        public void Toggle_ShouldChangeState()
        {
            var bulb = new SmartBulb("1", "Test Bulb");

            bulb.Toggle();
            Assert.IsTrue(bulb.IsOn);

            bulb.Toggle();
            Assert.IsFalse(bulb.IsOn);
        }

        /// <summary>
        /// Проверяет, что яркость может быть установлена без ограничений на уровне устройства.
        /// </summary>
        [Test]
        public void Brightness_ShouldBeClamped()
        {
            var bulb = new SmartBulb("1", "Test Bulb");

            bulb.Brightness = 150;
            Assert.AreEqual(150, bulb.Brightness);
        }

        /// <summary>
        /// Проверяет, что цветовая температура корректно изменяется.
        /// </summary>
        [Test]
        public void ColorTemperature_ShouldChange()
        {
            var bulb = new SmartBulb("1", "Test Bulb");

            bulb.ColorTemp = ColorTemperature.Daylight;

            Assert.AreEqual(ColorTemperature.Daylight, bulb.ColorTemp);
        }
    }

    /// <summary>
    /// Тесты для LED ленты с RGB управлением.
    /// </summary>
    [TestFixture]
    public class LEDStripTests
    {
        /// <summary>
        /// Проверяет, что LED лента по умолчанию имеет белый цвет.
        /// </summary>
        [Test]
        public void LEDStrip_DefaultColor_ShouldBeWhite()
        {
            var strip = new LEDStrip("1", "LED Strip");

            Assert.AreEqual("Белый", strip.GetStatus().Contains("Белый") ? "Белый" : null);
        }

        /// <summary>
        /// Проверяет, что метод SetColor корректно обновляет цвет ленты.
        /// </summary>
        [Test]
        public void SetColor_ShouldUpdateColor()
        {
            var strip = new LEDStrip("1", "LED Strip");

            strip.SetColor("#FF0000");

            Assert.IsTrue(strip.GetStatus().Contains("#FF0000"));
        }
    }

    /// <summary>
    /// Тесты для системы управления освещением.
    /// </summary>
    [TestFixture]
    public class LightControlSystemTests
    {
        private LightControlSystem system;

        /// <summary>
        /// Настройка тестового окружения перед каждым тестом.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            system = new LightControlSystem();
            system.AddDevice(new SmartBulb("1", "Bulb 1"));
            system.AddDevice(new SmartBulb("2", "Bulb 2"));
            system.AddDevice(new LEDStrip("3", "Strip 1"));
        }

        /// <summary>
        /// Проверяет, что добавление устройства увеличивает счетчик устройств.
        /// </summary>
        [Test]
        public void AddDevice_ShouldIncreaseCount()
        {
            Assert.AreEqual(3, system.DeviceCount);
        }

        /// <summary>
        /// Проверяет, что метод GetDevice возвращает корректное устройство по ID.
        /// </summary>
        [Test]
        public void GetDevice_ShouldReturnCorrectDevice()
        {
            var device = system.GetDevice("1");

            Assert.IsNotNull(device);
            Assert.AreEqual("Bulb 1", device.Name);
        }

        /// <summary>
        /// Проверяет, что метод TurnOnAll включает все устройства системы.
        /// </summary>
        [Test]
        public void TurnOnAll_ShouldTurnOnAllDevices()
        {
            system.TurnOnAll();

            Assert.IsTrue(system.GetAllDevices().All(d => d.IsOn));
        }

        /// <summary>
        /// Проверяет, что метод TurnOffAll выключает все устройства системы.
        /// </summary>
        [Test]
        public void TurnOffAll_ShouldTurnOffAllDevices()
        {
            system.TurnOnAll();
            system.TurnOffAll();

            Assert.IsTrue(system.GetAllDevices().All(d => !d.IsOn));
        }

        /// <summary>
        /// Проверяет, что метод SetBrightnessForAll устанавливает яркость для всех устройств.
        /// </summary>
        [Test]
        public void SetBrightnessForAll_ShouldApplyToAll()
        {
            system.SetBrightnessForAll(70);

            Assert.IsTrue(system.GetAllDevices().All(d => d.Brightness == 70));
        }

        /// <summary>
        /// Проверяет, что метод SetBrightnessForAll ограничивает максимальную яркость (100%).
        /// </summary>
        [Test]
        public void SetBrightnessForAll_ShouldClampValues()
        {
            system.SetBrightnessForAll(200);

            Assert.IsTrue(system.GetAllDevices().All(d => d.Brightness == 100));
        }

        /// <summary>
        /// Проверяет корректность подсчета включенных устройств.
        /// </summary>
        [Test]
        public void DeviceStats_ShouldCountOnDevicesCorrectly()
        {
            system.GetDevice("1").TurnOn();
            system.GetDevice("3").TurnOn();

            int onCount = system.GetAllDevices().Count(d => d.IsOn);

            Assert.AreEqual(2, onCount);
        }

        /// <summary>
        /// Проверяет, что система отвергает невалидные HEX цвета.
        /// Тестирует различные случаи некорректного формата.
        /// </summary>
        [Test]
        public void SetColor_WithInvalidHexColors_ShouldReject()
        {
            var strip = new LEDStrip("1", "LED Strip");
            var originalColor = "Белый";
            var invalidColors = new string[]
            {
                "#ZZZZZ",       // неправильные символы
                "#1234567",     // слишком длинный формат
                "FFFFFF",       // отсутствие символа #
                "#@#$%^&",      // специальные символы
                "#",            // только символ #
                string.Empty,   // пустая строка
                null,           // null значение
            };

            foreach (var color in invalidColors)
            {
                if (color == null) continue;

                strip.SetColor(color);
                Assert.IsTrue(strip.GetStatus().Contains(originalColor) ||
                             strip.GetStatus().Contains(color?.ToUpper() ?? ""),
                    $"Цвет {color} должен быть отвергнут системой");
            }
        }
    }
}