using System;
using System.Linq;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;
using System.Text;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadWriteService : IReadWriteService
    {
        #region Properties

        public IArmyService _armyService { get; set; }
        public IPlatoonService _platoonService { get; set; }
        public IGameService _gameService { get; set; }

        #endregion

        public ReadWriteService(IGameService game_service, IArmyService army_service, IPlatoonService platoon_service)
        {
            _platoonService = platoon_service;
            _armyService = army_service;
            _gameService = game_service;
        }
        
        /// <summary>
        /// Визуализация игры.
        /// </summary>
        public void Game()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            _gameService.Precondition();

            // Определили владельцев армий.
            Army human_army = _gameService.Nekropolis;
            Army computer_army = _gameService.Oplot;

            if (computer_army.Gamer.Equals(GameService.HUMAN_NAME))
            {
                human_army = _gameService.Oplot;
                computer_army = _gameService.Nekropolis;
            }

            // Визуализация армий.
            _armyService.Print(human_army);
            _armyService.Print(computer_army);

            // Сражение.
            while (human_army.PlatoonList.Any() && computer_army.PlatoonList.Any())
            {
                // Жертва.
                Platoon platoon_1;

                // Нападающий.
                Platoon platoon_2;

                // Выбор отрядов.
                if (_gameService.FirstName.Equals(GameService.HUMAN_NAME))
                {
                    // Жертва.
                    platoon_1 = _armyService.Get(computer_army, "Какой отряд врага будет жертвой?");

                    // Нападающий.
                    platoon_2 = _armyService.Get(human_army, "Какой отряд будет наступать?");

                    _gameService.ShowBattle(computer_army, platoon_1, platoon_2);
                }
                else
                {
                    // Жертва.
                    platoon_1 = _armyService.GetHealthiestMilitaryUnit(human_army.PlatoonList.ToList());

                    // Нападающий.
                    platoon_2 = _armyService.GetOffensiveMilitaryUnit(computer_army.PlatoonList.ToList()).First();

                    _gameService.ShowBattle(human_army, platoon_1, platoon_2);
                }

                Console.WriteLine();
                Console.WriteLine("Результат после предыдущей битвы:");

                // Визуализация армий после сражения.
                _armyService.Print(human_army);
                _armyService.Print(computer_army);
            }

            if (human_army.PlatoonList.Any())
            {
                Console.WriteLine($"Выиграл {human_army.Gamer}.");
            }
            else
            {
                Console.WriteLine($"Выиграл {computer_army.Gamer}.");
            }

            Console.Read();
        }
    }
}
