using System;
using System.Collections.Generic;
using System.Linq;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;
using NLog;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// Сервис армии.
    /// </summary>
    public class ArmyService : IArmyService
    {
        public static Random RND = new Random();
        public IPlatoonService _platoonService { get; set; }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Инициализация структур.
        /// </summary>
        public ArmyService(IPlatoonService platoon_service)
        {
            _platoonService = platoon_service;
        }

        /// <summary>
        /// Создание армии.
        /// </summary>
        /// <param name="armyName">Название армии.</param>
        /// <returns>Армия.</returns>
        public Army Create(string armyName)
        {
            Army result;

            if (armyName == GameService.ARMY_NAME_OPLOT)
            {
                result = new Army
                {
                    Name = armyName,
                    PlatoonList = new List<Platoon>{
                        _platoonService.Create("Кентавр", RND.Next(5, 11), 100, RND.Next(20, 41)),
                        _platoonService.Create("Эльф", RND.Next(5, 11), 200, RND.Next(20, 31)),
                        _platoonService.Create("Пегас", RND.Next(5, 11), 50, RND.Next(5, 51))
                    }
                };

                logger.Trace($"Армия {GameService.ARMY_NAME_OPLOT} создана.");
                return result;
            }
            else
            {
                result = new Army
                {
                    Name = armyName,
                    PlatoonList = new List<Platoon>{
                        _platoonService.Create("Скелет", RND.Next(5, 11), 150, RND.Next(15, 31)),
                        _platoonService.Create("Зомби", RND.Next(5, 11), 50, RND.Next(20, 31)),
                        _platoonService.Create("Вампир", RND.Next(5, 11), 150, RND.Next(10, 61))
                    }
                };

                logger.Trace($"Армия {GameService.ARMY_NAME_NECROPOLIS} создана.");
                return result;
            }
        }

        /// <summary>
        /// Выводит всю информацию об армии на экран.
        /// </summary>
        /// <param name="army">Армия.</param>
        public void Print(Army army)
        {
            Console.WriteLine(army.Name);
            
            List<Platoon> platoonList = army.PlatoonList;

            platoonList.ForEach(p =>
            {
                if (p.UnitList.Any())
                {
                    Console.WriteLine($"Отряд: {p.Name}");

                    int i = 0;
                    p.UnitList.ForEach(u =>
                    {
                        Console.WriteLine($"ID: {i}, жизни: {u.Life}, урон: {u.Casualties}");
                        i++;
                    });

                    Console.WriteLine();
                }
            });

            logger.Trace($"Информация об армии {army.Name} выведена на консоль.");
        }

        /// <summary>
        /// Проверяет наличие отряда в армии по названию отряда.
        /// </summary>
        /// <param name="army">Армия.</param>
        /// <param name="name">Название отряда.</param>
        /// <returns>Признак наличия отряда в армии.</returns>
        public bool IsPlatoon(Army army, string name)
        {
            return army.PlatoonList.Exists(p => p.Name == name);
        }

        /// <summary>
        /// Возвращает отряд по его имени.
        /// </summary>
        /// <param name="army">Армия.</param>
        /// <param name="text">Сообщение для пользователя.</param>
        /// <returns>Отряд.</returns>
        public Platoon Get(Army army, string text)
        {
            string platoon_name = "";

            // Ждем от пользователя корректное название отряда.
            while (!IsPlatoon(army, platoon_name))
            {
                Console.WriteLine(text);
                platoon_name = Convert.ToString(Console.ReadLine());
            }

            var result = army.PlatoonList.FirstOrDefault(p => p.Name == platoon_name);

            logger.Trace($"Найденный отряд: {result.Name}.");

            return result;
        }

        /// <summary>
        /// Возвращает список самых сильных отрядов.
        /// Используется компьютером.
        /// Выбираем самого сильного, то есть того, чья суммарная сила урона максимальна.
        /// </summary>
        /// <param name="platoonList">Список отрядов.</param>
        /// <returns>Список отрядов.</returns>
        public IEnumerable<Platoon> GetOffensiveMilitaryUnit(List<Platoon> platoonList)
        {
            platoonList.ForEach(p =>
            {
                _platoonService.GetCasualties(p);
            });

            int max = platoonList.Max(p => p.Force);

            var result = platoonList.Where(p => p.Force == max);

            logger.Trace($"Выбран список самых сильных отрядов для компьютера, где сила удара = {max}. Их количество: {result.Count()}");

            return result;
        }

        /// <summary>
        /// Возвращает самый сильный и здоровый военный отряд противника для нападения на него.
        /// Самый здоровый отряд тот, где общая сумма жизни максимальна.
        /// Используется компьютером.
        /// </summary>
        /// <param name="platoonList">Список отрядов.</param>
        /// <returns>Самый здоровый и сильный отряд.</returns>
        public Platoon GetHealthiestMilitaryUnit(List<Platoon> platoonList)
        {
            List<Platoon> lst = GetOffensiveMilitaryUnit(platoonList).ToList();

            lst.ForEach(p =>
            {
                p.Life = p.UnitList.Sum(u => u.Life);
            });

            int max = platoonList.Max(p => p.Life);

            var result = platoonList.Where(p => p.Life == max).First();

            logger.Trace($"Выбран самый сильный и здоровый отряд для компьютера, где здоровье = {max}.");

            return result;
        }
    }
}
