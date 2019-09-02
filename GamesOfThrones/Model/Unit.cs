using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesOfThrones.Model
{
    /// <summary>
    /// Юнит.
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Очки жизни.
        /// Задается автоматически до начала игры.
        /// </summary>
        public int Life { get; set; }

        /// <summary>
        /// Очки урона, который юнит может нанести врагу.
        /// Задается автоматически до начала игры. Значение случайное.
        /// </summary>
        public int Casualties { get; set; }
    }
}
