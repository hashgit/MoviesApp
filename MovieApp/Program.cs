using Autofac;
using MovieApp.Manager;
using MovieApp.Manager.DataContext;
using MovieApp.Manager.Repositories;
using System;

namespace MovieApp
{
    class Program
    {
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MoviesManager>().As<IMoviesManager>().InstancePerDependency();
            builder.RegisterType<StatusRepository>().As<IStatusRepository>().InstancePerDependency();
            builder.RegisterType<MoviesRepository>().As<IMoviesRepository>().InstancePerDependency();
            builder.RegisterType<MoviesDbContext>().As<MoviesDbContext>().InstancePerLifetimeScope();


            return builder.Build();
        }

        static void Main(string[] args)
        {
            var container = BuildContainer();
            var manager = container.Resolve<IMoviesManager>();
            manager.SyncDatabase();

            Console.ReadKey();
        }
    }
}
