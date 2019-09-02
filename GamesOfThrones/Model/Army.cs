using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesOfThrones.Model
{
    /// <summary>
    /// Армия.
    /// </summary>
    public class Army
    {
        /// <summary>
        /// Название армии.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Владелец армии.
        /// </summary>
        public string Gamer { get; set; }

        /// <summary>
        /// Список из трех отрядов.
        /// </summary>
        public List<Platoon> PlatoonList { get; set; }
    }
}
