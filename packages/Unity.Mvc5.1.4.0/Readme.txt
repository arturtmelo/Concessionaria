using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Services;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Unity;
using Unity.Mvc5;
using Unity.Injection;
using Google.Apis.Auth.OAuth2;
using System.Web;

namespace ConcessionariaMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configurar Inje��o de Depend�ncia
            var container = new UnityContainer();

            // Registrar ApplicationDbContext para inje��o de depend�ncia
            container.RegisterType<ApplicationDbContext>();

            // Registrar CepService para inje��o de depend�ncia
            container.RegisterType<CepService>();

            // Registrar SheetsService para inje��o de depend�ncia com configura��o adicional
            container.RegisterType<SheetsService>(new InjectionFactory(c =>
            {
                // Configura��o do servi�o do Google Sheets
                var credential = GoogleCredential.FromFile(HttpContext.Current.Server.MapPath("~/App_Data/credentials"))
                                .CreateScoped(SheetsService.Scope.Spreadsheets);
                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "ConcessionariaMVC",
                });
            }));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
