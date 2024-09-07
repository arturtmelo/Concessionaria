using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Services;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Unity;
using Unity.Mvc5;
using Google.Apis.Auth.OAuth2;
using ConcessionariaMVC.Repositories;

namespace ConcessionariaMVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Injeção Dependência
            var container = new UnityContainer();
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<CepService>();
            container.RegisterType<IClienteRepository, ClienteRepository>();
            container.RegisterType<IVeiculoRepository, VeiculoRepository>();
            container.RegisterType<IFabricanteRepository, FabricanteRepository>();
            container.RegisterType<IConcessionariaRepository, ConcessionariaRepository>();
            container.RegisterFactory<SheetsService>(c =>
            {
                // G-Sheets
                var credential = GoogleCredential.FromFile(HttpContext.Current.Server.MapPath("~/App_Data/credentials"))
                    .CreateScoped(SheetsService.Scope.Spreadsheets);

                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "ConcessionariaMVC",
                });
            });

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        // Linguagem Global
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
        }
    }
}
