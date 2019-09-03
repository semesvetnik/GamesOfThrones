using GamesOfThrones.Services;
using Autofac;
using GamesOfThrones.Interfaces;
using NLog;
using System;

namespace GamesOfThrones
{
    public class Program
    {
        #region Properties

        static IGameService _gameService { get; set; }
        static IArmyService _armyService { get; set; }
        static IPlatoonService _platoonService { get; set; }
        static IContainer Container { get; set; }

        static IReadWriteService _readWriteService { get; set; }

        #endregion

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UnitService>().As<IUnitService>();
            builder.RegisterType<PlatoonService>().As<IPlatoonService>();
            builder.RegisterType<ArmyService>().As<IArmyService>();
            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<ReadWriteService>().As<IReadWriteService>();

            builder.Register(c => new PlatoonService(c.Resolve<IUnitService>())).As<IPlatoonService>();
            builder.Register(c => new ArmyService(c.Resolve<IPlatoonService>())).As<IArmyService>();
            builder.Register(c => new GameService(c.Resolve<IArmyService>(), c.Resolve<IPlatoonService>())).As<IGameService>();
            builder.Register(c => new ReadWriteService(c.Resolve<IGameService>(), 
                c.Resolve<IArmyService>(), c.Resolve<IPlatoonService>())).As<IReadWriteService>();

            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                _platoonService = scope.Resolve<IPlatoonService>();
                _armyService = scope.Resolve<IArmyService>();
                _gameService = scope.Resolve<IGameService>();
                _readWriteService = scope.Resolve<IReadWriteService>();
            }

            _readWriteService.Game();
        }
    }
}
