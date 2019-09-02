using System.Linq;
using GamesOfThrones.Model;
using GamesOfThrones.Interfaces;

namespace GamesOfThrones.Services
{
    /// <summary>
    /// Сервис отряда.
    /// </summary>
    public class PlatoonService : IPlatoonService
    {
        /// <summary>
        /// Сервис юнита.
        /// </summary>
        public IUnitService _unitService { get; set; }

        /// <summary>
        /// Инициализация структур.
        /// </summary>
        public PlatoonService(IUnitService unit_service)
        {
            _unitService = unit_service;
        }

        /// <summary>
        /// Создает отряд по указанным параметрам.
        /// </summary>
        /// <param name="name">Название отряда.</param>
        /// <param name="count">Количество юнитов в отряде.</param>
        /// <param name="life">Значение жизни юнита.</param>
        /// <param name="casualties">Значение урона юнита.</param>
        /// <returns>Отряд.</returns>
        public Platoon Create(string name, int count, int life, int casualties)
        {
            return new Platoon
            {
                Name = name,
                Count = count,
                Life = life,
                Casualties = casualties,
                UnitList = _unitService.GetUnitList(count, life, casualties).ToList()
            };
        }

        /// <summary>
        /// Возвращает силу урона отряда.
        /// </summary>
        /// <param name="platoon">Отряд.</param>
        /// <returns>Сила урона.</returns>
        public int GetCasualties(Platoon platoon)
        {
            int casualties = platoon.UnitList.Sum(u=> u.Casualties);
            platoon.Force = casualties;

            return casualties;
        }
    }
}
