using System.Collections.Generic;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// Сервис юнита.
    /// </summary>
    public class UnitService : IUnitService
    {
        /// <summary>
        /// Формирует список юнитов с известными значениями жизни и урона.
        /// </summary>
        /// <param name="count">Количество элементов списка.</param>
        /// <param name="life">Значение жизни юнита.</param>
        /// <param name="casualties">Значение урона юнита.</param>
        /// <returns>Список юнитов.</returns>
        public IEnumerable<Unit> GetUnitList(int count, int life, int casualties)
        {
            var result = new List<Unit>();
            for (int i = 0; i < count; i++)
            {
                result.Add(new Unit
                {
                    Life = life,
                    Casualties = casualties
                });
            }

            return result;
        }
    }
}
