using GamesOfThrones.Model;

namespace GamesOfThrones.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса отряда.
    /// </summary>
    public interface IPlatoonService
    {
        Platoon Create(string name, int count, int life, int casualties);
        int GetCasualties(Platoon platoon);
    }
}