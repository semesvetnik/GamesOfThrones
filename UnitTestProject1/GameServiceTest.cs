using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamesOfThrones.Interfaces;
using GamesOfThrones.Services;
using GamesOfThrones.Model;

namespace GamesOfThronesTests
{
    /// <summary>
    /// Проверяет методы сервиса игры.
    /// </summary>
    [TestClass]
    public class GameServiceTest
    {
        #region Properties

        public IGameService _gameService { get; set; }

        public IArmyService _armyService { get; set; }

        public IPlatoonService _platoonService { get; set; }

        public IUnitService _unitService { get; set; }

        public GameServiceTest()
        {
            _unitService = new UnitService();
            _platoonService = new PlatoonService(_unitService); 
            _armyService = new ArmyService(_platoonService); 
            _gameService = new GameService(_armyService, _platoonService);
        }

        #endregion

        #region Precondition

        [TestMethod]
        public void Precondition_()
        {

        }

        #endregion

        #region Offensive

        /// <summary>
        /// Проверяет, что сумма жизни юнитов отряда-жертвы уменьшается.
        /// Условия: количество юнитов в отрядах одинаковое, суммарная сила нападающего отряда больше суммарной жизни отряда-жертвы.
        /// </summary>
        [TestMethod]
        public void Offensive_UnitLife_Decreased()
        {
            Platoon pl_1 = _platoonService.Create("Кентавр", 3, 150, 60);
            Platoon pl_2 = _platoonService.Create("Скелет", 3, 40, 10);

            int life_sum_1 = pl_2.UnitList.Sum(u=> u.Life);

            Platoon result = _gameService.Offensive(pl_1, pl_2);

            int life_sum_2 = pl_2.UnitList.Sum(u=> u.Life);

            Assert.IsTrue(life_sum_2 < life_sum_1, 
                String.Format("Сумма жизней юнитов отряда-жертвы не изменилась. life_sum = {0}", life_sum_2));
        }

        /// <summary>
        /// Проверяет, что количество отрядов-жертвы уменьшается. 
        /// Принцип Дирихле.
        /// Условия: сила удара нападающего отряда больше жизни отряда-жертвы.
        /// </summary>
        [TestMethod]
        public void Offensive_UnitCount_Decreased()
        {
            Platoon pl_1 = _platoonService.Create("Кентавр", 3, 70, 33);
            pl_1.UnitList[0].Casualties = 34; //34 33 33

            Platoon pl_2 = _platoonService.Create("Скелет", 3, 33, 10);

            Platoon result = _gameService.Offensive(pl_1, pl_2);

            int life_sum_2 = pl_2.UnitList.Sum(u => u.Life);

            Assert.IsTrue(pl_2.UnitList.Count < 3,
                String.Format("Количество юнитов отряда-жертвы не изменилось. pl_2.UnitList.Count = {0}", pl_2.UnitList.Count)); 
        }

        #endregion 

        #region ShowBattle

        /// <summary>
        /// Проверяет смену игрока после битвы.
        /// </summary>
        [TestMethod]
        public void ShowBattle_FirstName_Change()
        {
            _gameService.FirstName = GameService.COMPUTER_NAME;

            // Армия жертвы.
            Army army_1 = _armyService.Create(GameService.ARMY_NAME_OPLOT);
            army_1.Gamer = GameService.HUMAN_NAME;

            Army army_2 = _armyService.Create(GameService.ARMY_NAME_NECROPOLIS);
            army_2.Gamer = GameService.COMPUTER_NAME;

            _gameService.ShowBattle(army_1, army_2.PlatoonList[0], army_1.PlatoonList[0]);

            Assert.IsTrue(_gameService.FirstName == GameService.HUMAN_NAME);
        }

        /// <summary>
        /// Проверяет удаление отряда после битвы.
        /// </summary>
        [TestMethod]
        public void ShowBattle_PlatoonList_Change()
        {
            var army = new Army
            {
                Name = GameService.ARMY_NAME_OPLOT,
                PlatoonList = new List<Platoon>{
                        _platoonService.Create("Кентавр", 1, 100, 40),
                        _platoonService.Create("Эльф", 3, 200, 30)
                    }
            };

            var platoon = _platoonService.Create("Скелет", 3, 300, 100);

            _gameService.ShowBattle(army, army.PlatoonList[0], platoon);

            Assert.IsTrue(army.PlatoonList.Count() == 1);
        }

        #endregion 
    }
}
