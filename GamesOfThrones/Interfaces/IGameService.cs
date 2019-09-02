using GamesOfThrones.Model;

namespace GamesOfThrones.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса игры.
    /// </summary>
    public interface IGameService
    {
        string FirstName { get; set; }
        Army Nekropolis { get; set; }
        Army Oplot { get; set; }

        Platoon Offensive(Platoon pl_1, Platoon pl_2);
        void Precondition();
        void ShowBattle(Army army, Platoon platoon_1, Platoon platoon_2);
    }
}