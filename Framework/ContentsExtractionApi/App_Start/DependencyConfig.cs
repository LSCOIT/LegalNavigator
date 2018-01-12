using Autofac;
using Autofac.Integration.WebApi;
using ContentDataAccess.DataContextFactory;
using ContentDataAccess;
using System.Reflection;
using System.Web.Http;

namespace ContentsExtractionApi.App_Start
{
    /// <summary>
    /// DependencyConfig
    /// </summary>
    public class DependencyConfig
    {
        public static void  Register(HttpConfiguration config)
        {

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ContentDataRepository>().As<IContentDataRepository>();
            
            builder.RegisterType<CrowledContentDataContextFactory>().As<ICrowledContentDataContextFactory>();
            
            var builtContainer = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(builtContainer);

        }
    }
}