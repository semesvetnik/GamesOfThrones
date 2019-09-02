using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamesOfThrones.Services;
using GamesOfThrones.Interfaces;

namespace GamesOfThronesTests
{
    /// <summary>
    /// Summary description for PlatoonServiceTest
    /// </summary>
    [TestClass]
    public class PlatoonServiceTest
    {
        public IUnitService _unitService { get; set; }
        public IPlatoonService _platoonService { get; set; }

        public PlatoonServiceTest()
        {
            _unitService = new UnitService();
            _platoonService = new PlatoonService(_unitService);
        }

        /// <summary>
        /// Проверяет, что отряд создается со списком юнитов.
        /// </summary>
        [TestMethod]
        public void Create_Platoon_Crate()
        {
            var result = _platoonService.Create("One", 3, 100, 33);
            var list = result.UnitList;

            Assert.AreEqual(result.Name, "One");
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result.Life, 100);
            Assert.AreEqual(result.Casualties, 33);

            // Здесь можно использовать моки.
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 3);
            Assert.IsTrue(list.All(u => u.Life == 100));
            Assert.IsTrue(list.All(u => u.Casualties == 33));
        }

        /// <summary>
        /// Проверяет вычисление суммы удара отряда.
        /// </summary>
        [TestMethod]
        public void GetCasualties_Casualties_Sum()
        {
            var platoon = _platoonService.Create("One", 3, 100, 20);
            var result = _platoonService.GetCasualties(platoon);

            Assert.IsTrue(result == platoon.Count * platoon.Casualties);
        }
    }
}
