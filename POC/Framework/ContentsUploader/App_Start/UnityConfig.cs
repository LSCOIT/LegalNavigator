using ContentDataAccess;
using ContentDataAccess.DataContextFactory;
using ContentsUploader.ContentUploadManager;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ContentsUploader
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IExcelUploadManager, ExcelUploadManager>();
            container.RegisterType<IContentDataRepository, ContentDataRepository>();
            container.RegisterType<ICrowledContentDataContextFactory, CrowledContentDataContextFactory>();
            


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}