using System;

namespace PizzaAsteroidApp
{
    public class PizzaAsteroid
    {
        public string Name;
        public CrustType Crust;
        public double DiameterKm;
        public int TemperatureCelsius;
        public bool HasExtraCheese;

        private DateTime discoveryDate;

        public void SetDiscoveryDate(DateTime date)
        {
            discoveryDate = date;
        }

        public DateTime GetDiscoveryDate()
        {
            return discoveryDate;
        }

        public PizzaAsteroid() { }

        public PizzaAsteroid(string name, CrustType crust, double diameterKm, int temperatureCelsius, bool hasExtraCheese, DateTime discoveryDate)
        {
            Name = name;
            Crust = crust;
            DiameterKm = diameterKm;
            TemperatureCelsius = temperatureCelsius;
            HasExtraCheese = hasExtraCheese;
            this.discoveryDate = discoveryDate;
        }

        public void HeatUp(int temperature)
        {
            TemperatureCelsius += temperature;
            Console.WriteLine($"Астероїд '{Name}' нагрівся на {temperature}°C. Поточна температура: {TemperatureCelsius}°C.");
            if (TemperatureCelsius > 60 && HasExtraCheese)
            {
                Console.WriteLine("УВАГА! Сир на поверхні розплавився і утворив захисну мантію-скоринку!");
            }
        }

        public void Slice(int slices)
        {
            if (slices <= 1)
            {
                Console.WriteLine($"[!] Астероїд '{Name}' неможливо нарізати на {slices} шматочків.");
                return;
            }
            DiameterKm /= Math.Sqrt(slices);
            Console.WriteLine($"[!] Астероїд '{Name}' розділено на {slices} шматків! Новий діаметр кожного шматка: {DiameterKm:F2} км.");
        }

        public string CollideWithTarget(string targetName)
        {
            return $"УВАГА! ЗІТКНЕННЯ!!! '{Name}' врізається в об'єкт '{targetName}', покриваючи його товстим шаром соусу, астероїд розлітається на обломки типу {Crust}!";
        }
    }
}