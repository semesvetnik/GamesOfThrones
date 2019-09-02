using System;
using System.Collections.Generic;
using System.Linq;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// Сервис игры.
    /// </summary>
    public class GameService : IGameService  
    {
        #region Consts

        public const string HUMAN_NAME = "Человек";
        public const string COMPUTER_NAME = "Компьютер";

        public const string ARMY_NAME_OPLOT = "Оплот";
        public const string ARMY_NAME_NECROPOLIS = "Некрополис";

        #endregion 

        #region Properties

        /// <summary>
        /// Армия Некрополис.
        /// </summary>
        public Army Nekropolis { get; set; }

        /// <summary>
        /// Армия Оплот.
        /// </summary>
        public Army Oplot { get; set; }

        /// <summary>
        /// Имя того, кто начинает играть.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Сервис армии.
        /// </summary>
        public IArmyService _armyService { get; set; }

        /// <summary>
        /// Сервис отряда.
        /// </summary>
        public IPlatoonService _platoonService { get; set; }

        /// <summary>
        /// Инициализация структур.
        /// </summary>
        public GameService(IArmyService army_service, IPlatoonService platoon_service)
        {
            _armyService = army_service;
            _platoonService = platoon_service;
        }

        #endregion Properties

        /// <summary>
        /// Выполняет предусловия игры.
        /// Создание армий, их распределение по игрокам, определение очередности хода.
        /// </summary>
        public void Precondition()
        {
            Nekropolis = _armyService.Create(ARMY_NAME_NECROPOLIS);
            Oplot = _armyService.Create(ARMY_NAME_OPLOT);             

            // 1) Определить право первого хода - компьютер или человек.
            if (ArmyService.RND.Next(2) == 0)
            {
                FirstName = HUMAN_NAME;
                Console.WriteLine("Первым ходит Человек.");

                // 2) Выбрать замок.
                string n = Console.ReadLine();

                while (n != "0" && n != "1")
                {
                    Console.WriteLine($"Выберите свой замок, {ARMY_NAME_OPLOT} = 0 или {ARMY_NAME_NECROPOLIS} = 1.");
                    n = Console.ReadLine();
                }

                int number = Convert.ToInt32(n);

                if (number == 1)
                {
                    Console.WriteLine($"Замок Компьютера: {ARMY_NAME_OPLOT}. Замок Человека: {ARMY_NAME_NECROPOLIS}.");

                    Nekropolis.Gamer = HUMAN_NAME;
                    Oplot.Gamer = COMPUTER_NAME;
                }
                else
                {
                    Console.WriteLine($"Замок Компьютера: {ARMY_NAME_NECROPOLIS}. Замок Человека: {ARMY_NAME_OPLOT}.");

                    Nekropolis.Gamer = COMPUTER_NAME;
                    Oplot.Gamer = HUMAN_NAME;
                }
            }
            else
            {
                FirstName = COMPUTER_NAME;
                Console.WriteLine("Первым ходит Компьютер.");

                if (ArmyService.RND.Next(2) == 0)
                {
                    Console.WriteLine($"Замок Компьютера: {ARMY_NAME_OPLOT}. Замок Человека: {ARMY_NAME_NECROPOLIS}.");

                    Nekropolis.Gamer = HUMAN_NAME;
                    Oplot.Gamer = COMPUTER_NAME;
                }
                else
                {
                    Console.WriteLine($"Замок Человека: {ARMY_NAME_OPLOT}. Замок Компьютера: {ARMY_NAME_NECROPOLIS}.");

                    Nekropolis.Gamer = COMPUTER_NAME;
                    Oplot.Gamer = HUMAN_NAME;
                }
            }             
        }

        /// <summary>
        /// Наступление первого отряда на второй.
        /// </summary>
        /// <param name="pl_1">Нападающий отряд.</param>
        /// <param name="pl_2">Обороняющийся отряд.</param>
        /// <returns>Обороняющийся отряд.</returns>
        public Platoon Offensive(Platoon pl_1, Platoon pl_2)
        {
            List<Unit> result = new List<Unit>();

            // Урон, который может нанести отряд.
            int сasualties = _platoonService.GetCasualties(pl_1);

            var lst = pl_2.UnitList;

            int t = 0;
            int c = сasualties;

            while (lst.Count > 1)
            {
                // Индекс случайно выбранной жертвы.
                int index = ArmyService.RND.Next(lst.Count());

                // Величина удара.
                t = ArmyService.RND.Next(c + 1);

                // Нанесение удара.
                lst[index].Life -= t;
                c -= t;

                if (lst[index].Life > 0)
                    result.Add(lst[index]);

                lst.RemoveAt(index);
            }

            // Удар по последнему юниту.
            lst[0].Life -= c;

            if (lst[0].Life > 0)
                result.Add(lst[0]);

            pl_2.UnitList = result;

            return pl_2;
        }

        /// <summary>
        /// Демонстрация битвы.
        /// </summary>
        /// <param name="army">Армия жертвы.</param>
        /// <param name="platoon_1">Отряд-жертва.</param>
        /// <param name="platoon_2">Нападающий отряд.</param>
        public void ShowBattle(Army army, Platoon platoon_1, Platoon platoon_2)
        {
            Console.WriteLine($"Отряд {platoon_2.Name} нападал на отряд {platoon_1.Name}.");
            Console.WriteLine();

            // Жертва после боя.
            Platoon pl = Offensive(platoon_2, platoon_1);

            if (!pl.UnitList.Any())
            {
                army.PlatoonList.Remove(pl);
            }

            FirstName = army.Gamer;
            Console.WriteLine($"Теперь играет {FirstName}.");
        }
    }
}
