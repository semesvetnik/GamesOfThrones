using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamesOfThrones.Interfaces;
using GamesOfThrones.Services;
using GamesOfThrones.Model;

namespace GamesOfThronesTests
{
    /// <summary>
    /// Проверяет методы сервиса юнита.
    /// </summary>
    [TestClass]
    public class UnitServiceTest
    {
        public IUnitService _unitService;

        public UnitServiceTest()
        {
            _unitService = new UnitService();
        }

        /// <summary>
        /// Проверяет, что список юнитов создается.
        /// </summary>
        [TestMethod]
        public void GetUnitList_UnitList_Create()
        {
            var result = _unitService.GetUnitList(3, 33, 22);

            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Count() == 3);
            Assert.IsTrue(result.All(u => u.Life == 33));
            Assert.IsTrue(result.All(u => u.Casualties == 22));
        }

        /// <summary>
        /// Проверяет, что список юнитов пуст.
        /// </summary>
        [TestMethod]
        public void GetUnitList_UnitList_Empty()
        {
            var result = _unitService.GetUnitList(0, 33, 22);
            var result_1 = new List<Unit>();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
    }
}
