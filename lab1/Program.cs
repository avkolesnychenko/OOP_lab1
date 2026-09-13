using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PizzaAsteroidApp
{
    internal class Program
    {
        private static readonly List<PizzaAsteroid> Asteroids = new List<PizzaAsteroid>();
        private static int _maxCapacity;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            _maxCapacity = ReadPositiveInt("Введіть максимальну кількість астероїдів N (N > 0): ");

            bool running = true;
            while (running)
            {
                Console.WriteLine($"Заповненість: {Asteroids.Count} / {_maxCapacity}");
                Console.WriteLine("1 – Додати об'єкт");
                Console.WriteLine("2 – Переглянути всі об'єкти");
                Console.WriteLine("3 – Знайти об'єкт");
                Console.WriteLine("4 – Продемонструвати поведінку");
                Console.WriteLine("5 – Видалити об'єкт");
                Console.WriteLine("0 – Вийти з програми");
                Console.Write(">");

                string ans = Console.ReadLine().Trim();
                Console.WriteLine();

                switch (ans)
                {
                    case "1":
                        AddAsteroidMenu();
                        break;
                    case "2":
                        PrintTable(Asteroids, "Список усіх піцца-астероїдів");
                        break;
                    case "3":
                        SearchAsteroids();
                        break;
                    case "4":
                        DemonstrateBehavior();
                        break;
                    case "5":
                        DeleteAsteroidMenu();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Робота програми завершена. Гарного космічного апетиту!");
                        break;
                    default:
                        Console.WriteLine("Помилка! Невідомий пункт меню. Спробуйте ще раз.");
                        break;
                }
            }
        }

        // Adding object
        private static void AddAsteroidMenu()
        {
            if (Asteroids.Count >= _maxCapacity)
            {
                Console.WriteLine($"Досягнуто ліміт N = {_maxCapacity}. Неможливо додати новий об'єкт.");
                return;
            }

            Console.WriteLine("Режим додавання:");
            Console.WriteLine("1 - Ввести дані вручну");
            Console.WriteLine("2 - Згенерувати автоматично");
            Console.Write(">");
            string mode = Console.ReadLine().Trim();

            if (mode == "1")
            {
                PizzaAsteroid asteroid = new PizzaAsteroid
                {
                    Name = ReadValidatedName(),
                    Crust = ReadValidatedCrust(),
                    DiameterKm = ReadValidatedDiameter(),
                    TemperatureCelsius = ReadValidatedTemperature(),
                    HasExtraCheese = ReadValidatedBool("Чи є подвійний сир? (1/так - true, 0/ні - false): ")
                };
                asteroid.SetDiscoveryDate(ReadValidatedDiscoveryDate());

                Asteroids.Add(asteroid);
                Console.WriteLine($"Успіх! Астероїд '{asteroid.Name}' успішно створено та додано!");
            }
            else if (mode == "2")
            {
                var rand = new Random();
                string[] names = { "Пепероні-X", "Квадро-Формаджо", "Карбонара-99", "Гаваї-Ультра", "Діавола-Prime" };
                var asteroid = new PizzaAsteroid(
                    name: names[rand.Next(names.Length)] + "-" + rand.Next(10, 999),
                    crust: (CrustType)rand.Next(1, 5),
                    diameterKm: Math.Round(rand.NextDouble() * 99 + 1, 2),
                    temperatureCelsius: rand.Next(-200, 350),
                    hasExtraCheese: rand.Next(2) == 1,
                    discoveryDate: DateTime.Now.AddDays(-rand.Next(1, 5000))
                );

                Asteroids.Add(asteroid);
                Console.WriteLine($"Успіх! Автоматично згенеровано та додано: '{asteroid.Name}'.");
            }
            else
            {
                Console.WriteLine("Помилка! Некоректний вибір режиму.");
            }
        }

        // validation
        private static string ReadValidatedName()
        {
            while (true)
            {
                Console.Write("Введіть назву астероїда (3-20 симв., літери/цифри/-): ");
                string input = Console.ReadLine().Trim();

                if (!string.IsNullOrEmpty(input) && input.Length >= 3 && input.Length <= 20 &&
                    Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯіІїЇєЄ0-9\s\-]+$"))
                {
                    return input;
                }
                Console.WriteLine("Помилка! Некоректна назва! Перевірте довжину та неприпустимі символи.");
            }
        }

        private static CrustType ReadValidatedCrust()
        {
            while (true)
            {
                Console.WriteLine("Оберіть бортик/тісто: 1 - Thin, 2 - CheeseStuffed, 3 - DeepDish, 4 - Classic");
                Console.Write("Введіть номер (1-4): ");
                if (int.TryParse(Console.ReadLine(), out int val) && Enum.IsDefined(typeof(CrustType), val))
                {
                    return (CrustType)val;
                }
                Console.WriteLine("Помилка! Оберіть значення від 1 до 4.");
            }
        }

        private static double ReadValidatedDiameter()
        {
            while (true)
            {
                Console.Write("Введіть діаметр у км [0.1 .. 1000.0]: ");
                if (double.TryParse(Console.ReadLine().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double val)
                    && val >= 0.1 && val <= 1000.0)
                {
                    return Math.Round(val, 2);
                }
                Console.WriteLine("Помилка! Діаметр має бути числом від 0.1 до 1000.0.");
            }
        }

        private static int ReadValidatedTemperature()
        {
            while (true)
            {
                Console.Write("Введіть температуру в °C [-273 .. 500]: ");
                if (int.TryParse(Console.ReadLine(), out int val) && val >= -273 && val <= 500)
                {
                    return val;
                }
                Console.WriteLine("Помилка! Температура повинна бути цілим числом від -273 до 500.");
            }
        }

        private static bool ReadValidatedBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().Trim().ToLower();
                if (input == "1" || input == "так" || input == "true") return true;
                if (input == "0" || input == "ні" || input == "false") return false;
                Console.WriteLine("Помилка! Введіть 1/так або 0/ні.");
            }
        }

        private static DateTime ReadValidatedDiscoveryDate()
        {
            DateTime minDate = new DateTime(1990, 1, 1);
            while (true)
            {
                Console.Write("Введіть дату відкриття (dd.MM.yyyy, від 01.01.1990 до сьогодні): ");
                string input = Console.ReadLine().Trim();

                if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    if (parsedDate >= minDate && parsedDate <= DateTime.Now.Date)
                    {
                        return parsedDate;
                    }
                    Console.WriteLine("Помилка! Дата виходить за дозволений діапазон (01.01.1990 — сьогодні).");
                }
                else
                {
                    Console.WriteLine("Помилка! Невірний формат дати! Використовуйте dd.MM.yyyy (наприклад, 15.08.2021).");
                }
            }
        }

        private static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    return n;
                }
                Console.WriteLine("Помилка! Число повинно бути цілим і більшим за нуль.");
            }
        }

        //table showing
        private static void PrintTable(List<PizzaAsteroid> list, string title)
        {
            if (list == null || list.Count == 0)
            {
                Console.WriteLine("Жодного об'єкта не знайдено.");
                return;
            }

            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"| {"#",-3} | {"Назва",-18} | {"Бортик",-14} | {"Діаметр (км)",-12} | {"T (°C)",-7} | {"Сир+",-6} | {"Дата відкриття",-14} |");
            Console.WriteLine(new string('-', 100));

            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i];
                Console.WriteLine($"| {i + 1,-3} | {a.Name,-18} | {a.Crust,-14} | {a.DiameterKm,-12:F2} | {a.TemperatureCelsius,-7} | {(a.HasExtraCheese ? "Так" : "Ні"),-6} | {a.GetDiscoveryDate(),-14:dd.MM.yyyy} |");
            }
            Console.WriteLine(new string('-', 100));
        }

        // searching
        private static void SearchAsteroids()
        {
            if (Asteroids.Count == 0)
            {
                Console.WriteLine("Список порожній, пошук неможливий.");
                return;
            }

            Console.WriteLine("Оберіть критерій пошуку:");
            Console.WriteLine("1 – За типом бортика (Crust)");
            Console.WriteLine("2 – За наявністю подвійного сиру (HasExtraCheese)");
            Console.Write(">");
            string subChoice = Console.ReadLine().Trim();

            List<PizzaAsteroid> results = new List<PizzaAsteroid>();

            if (subChoice == "1")
            {
                CrustType crust = ReadValidatedCrust();
                results = Asteroids.FindAll(a => a.Crust == crust);
            }
            else if (subChoice == "2")
            {
                bool cheese = ReadValidatedBool("Шукати з додатковим сиром? (1/так, 0/ні): ");
                results = Asteroids.FindAll(a => a.HasExtraCheese == cheese);
            }
            else
            {
                Console.WriteLine("Помилка! Невірний критерій пошуку.");
                return;
            }

            PrintTable(results, "Результати пошуку");
        }

        // object behavior
        private static void DemonstrateBehavior()
        {
            if (Asteroids.Count == 0)
            {
                Console.WriteLine("Немає об'єктів для виклику методів.");
                return;
            }

            PrintTable(Asteroids, "Оберіть астероїд для демонстрації");
            Console.Write($"Введіть порядковий номер об'єкта (1..{Asteroids.Count}): ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > Asteroids.Count)
            {
                Console.WriteLine("Помилка! Некоректний номер.");
                return;
            }

            PizzaAsteroid target = Asteroids[idx - 1];

            Console.WriteLine($"\nОберіть дію для астероїда '{target.Name}':");
            Console.WriteLine("1 – Нагріти астероїд (HeatUp)");
            Console.WriteLine("2 – Розрізати астероїд на шматки (Slice)");
            Console.WriteLine("3 – Створити зіткнення з планетою (CollideWithTarget)");
            Console.Write(">");

            switch (Console.ReadLine().Trim())
            {
                case "1":
                    Console.Write("На скільки градусів підняти температуру?: ");
                    if (int.TryParse(Console.ReadLine(), out int deg))
                    {
                        target.HeatUp(deg);
                    }
                    else
                    {
                        Console.WriteLine("Помилка! Некоректне число.");
                    }
                    break;
                case "2":
                    Console.Write("На скільки частин розрізати (>= 2)?: ");
                    if (int.TryParse(Console.ReadLine(), out int slices) && slices >= 2)
                    {
                        target.Slice(slices);
                    }
                    else
                    {
                        Console.WriteLine("Помилка! Кількість має бути цілим числом >= 2.");
                    }
                    break;
                case "3":
                    Console.Write("Введіть назву планети або супутника: ");
                    string planet = Console.ReadLine().Trim();
                    Console.WriteLine(target.CollideWithTarget(string.IsNullOrWhiteSpace(planet) ? "Марс" : planet));
                    break;
                default:
                    Console.WriteLine("Помилка! Невідомий метод.");
                    break;
            }
        }

        // deleting object
        private static void DeleteAsteroidMenu()
        {
            if (Asteroids.Count == 0)
            {
                Console.WriteLine("Список порожній, видалення неможливе.");
                return;
            }

            Console.WriteLine("Спосіб видалення:");
            Console.WriteLine("1 – За порядковим номером у таблиці");
            Console.WriteLine("2 – За назвою (будуть видалені всі збіги)");
            Console.Write(">");
            string choice = Console.ReadLine().Trim();

            if (choice == "1")
            {
                PrintTable(Asteroids, "Поточний перелік астероїдів");
                Console.Write($"Введіть номер для видалення (1..{Asteroids.Count}): ");
                if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= Asteroids.Count)
                {
                    string removedName = Asteroids[index - 1].Name;
                    Asteroids.RemoveAt(index - 1);
                    Console.WriteLine($"Успіх! Астероїд '{removedName}' успішно видалено.");
                }
                else
                {
                    Console.WriteLine("Помилка! Некоректний порядковий номер.");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Введіть назву для пошуку та видалення: ");
                string searchName = Console.ReadLine().Trim();

                int removedCount = Asteroids.RemoveAll(a => a.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
                if (removedCount > 0)
                {
                    Console.WriteLine($"Успіх! Видалено об'єктів: {removedCount}.");
                }
                else
                {
                    Console.WriteLine($"Об'єктів з назвою '{searchName}' не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Помилка! Некоректний вибір.");
            }
        }
    }
}