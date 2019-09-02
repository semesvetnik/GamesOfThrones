using System.Collections.Generic;
using GamesOfThrones.Model;

namespace GamesOfThrones.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса юнита.
    /// </summary>
    public interface IUnitService
    {
        IEnumerable<Unit> GetUnitList(int count, int life, int casualties);
    }
}
