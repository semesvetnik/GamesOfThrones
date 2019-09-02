using System;
using System.Collections.Generic;
using System.Linq;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// Сервис армии.
    /// </summary>
    public class ArmyService : IArmyService
    {
        public static Random RND = new Random();
        public IPlatoonService _platoonService { get; set; }

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
                return result = new Army
                {
                    Name = armyName,
                    PlatoonList = new List<Platoon>{
                        _platoonService.Create("Кентавр", RND.Next(5, 11), 100, RND.Next(20, 41)),
                        _platoonService.Create("Эльф", RND.Next(5, 11), 200, RND.Next(20, 31)),
                        _platoonService.Create("Пегас", RND.Next(5, 11), 50, RND.Next(5, 51))
                    }
                };
            else
                return result = new Army
                {
                    Name = armyName,
                    PlatoonList = new List<Platoon>{
                        _platoonService.Create("Скелет", RND.Next(5, 11), 150, RND.Next(15, 31)),
                        _platoonService.Create("Зомби", RND.Next(5, 11), 50, RND.Next(20, 31)),
                        _platoonService.Create("Вампир", RND.Next(5, 11), 150, RND.Next(10, 61))
                    }
                };
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

            while (!IsPlatoon(army, platoon_name))
            {
                Console.WriteLine(text);
                platoon_name = Convert.ToString(Console.ReadLine());
            }

            return army.PlatoonList.FirstOrDefault(p => p.Name == platoon_name);
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

            return platoonList.Where(p => p.Force == max);
        }

        /// <summary>
        /// Возвращает самый сильный и здоровый военный отряд противника для нападения на него.
        /// Самый здоровый отряд тот, где общая сумма жизни максимальна.
        /// Используется компьютером.
        /// </summary>
        /// <param name="platoonList">Список отрядов.</param>
        /// <returns>Самый здоровый и сльный отряд.</returns>
        public Platoon GetHealthiestMilitaryUnit(List<Platoon> platoonList)
        {
            List<Platoon> lst = GetOffensiveMilitaryUnit(platoonList).ToList();

            lst.ForEach(p =>
            {
                p.Life = p.UnitList.Sum(u => u.Life);
            });

            int max = platoonList.Max(p => p.Life);

            return platoonList.Where(p => p.Life == max).First();
        }
    }
}
