using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesOfThrones.Model
{
    /// <summary>
    /// Отряд.
    /// </summary>
    public class Platoon
    {
        /// <summary>
        /// Название отряда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество юнитов в отряде.
        /// Задается автоматически до начала игры.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Список юнитов.
        /// </summary>
        public List<Unit> UnitList { get; set; }

        /// <summary>
        /// Очки жизни одного юнита.
        /// Задается автоматически до начала игры.
        /// </summary>
        public int Life { get; set; }

        /// <summary>
        /// Урон, который юнит может нанести врагу.
        /// Задается автоматически до начала игры. Значение случайное.
        /// Измеряется в жизнях.
        /// </summary>
        public int Casualties { get; set; }

        /// <summary>
        /// Сила отряда.
        /// </summary>
        public int Force { get; set; }
    }
}
