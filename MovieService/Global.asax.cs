using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using MovieApp.Manager;
using MovieApp.Manager.Repositories;
using MovieApp.Manager.DataContext;

namespace MovieService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = BuildContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<MoviesManager>().As<IMoviesManager>().InstancePerRequest();
            builder.RegisterType<MoviesRepository>().As<IMoviesRepository>().InstancePerRequest();
            builder.RegisterType<StatusRepository>().As<IStatusRepository>().InstancePerRequest();
            builder.RegisterType<MoviesDbContext>().As<MoviesDbContext>().InstancePerRequest();

            return builder.Build();
        }
    }
}
