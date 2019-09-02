using System.Collections.Generic;
using GamesOfThrones.Model;
using GamesOfThrones.Services;

namespace GamesOfThrones.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса армии.
    /// </summary>
    public interface IArmyService
    {
        Army Create(string armyName);
        Platoon Get(Army army, string text);
        Platoon GetHealthiestMilitaryUnit(List<Platoon> platoonList);
        IEnumerable<Platoon> GetOffensiveMilitaryUnit(List<Platoon> platoonList);
        bool IsPlatoon(Army army, string name);
        void Print(Army army);
    }
}