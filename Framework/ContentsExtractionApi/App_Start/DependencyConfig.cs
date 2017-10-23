using Autofac;
using Autofac.Integration.WebApi;
using CrawledContentDataAccess.DataContextFactory;
using CrowledContentDataAccess;
using CrowledContentDataAccess.DataContextFactory;
using System.Reflection;
using System.Web.Http;

namespace ContentsExtractionApi.App_Start
{
    public class DependencyConfig
    {
        public static void  Register(HttpConfiguration config)
        {

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<CrowledContentDataRepository>().As<ICrowledContentDataRepository>();
            
            builder.RegisterType<CrowledContentDataContextFactory>().As<ICrowledContentDataContextFactory>();
            
            var builtContainer = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(builtContainer);

        }
    }
}