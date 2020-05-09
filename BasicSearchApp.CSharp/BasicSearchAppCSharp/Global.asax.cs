using BasicSearchApp.data;
using BasicSearchApp.Repositories;
using BasicSearchApp.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;

namespace BasicSearchAppCSharp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register your types, for instance using the scoped lifestyle:
            container.Register<IBasicSearchService, BasicSearchService>(Lifestyle.Scoped);
            container.Register<IResourceService, ResourceService>(Lifestyle.Scoped);
            container.Register<IIndexingService, IndexingService>(Lifestyle.Scoped);
            container.Register<IDocumentRepository, DocumentRepository>(Lifestyle.Scoped);
            container.Register<IPatientRepository, PatientRepository>(Lifestyle.Scoped);
            container.Register<IContentService, ContentService>(Lifestyle.Scoped);
            container.Register<IConfigurationService, ConfigurationService>(Lifestyle.Scoped);

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            IPatientRepository patientRepository = new PatientRepository();
            IDocumentRepository documentRepository = new DocumentRepository();
            IIndexingService indexingService = new IndexingService(documentRepository);
            IConfigurationService configuretionService = new ConfigurationService();

            InternalDatabase database = new InternalDatabase(patientRepository, documentRepository, indexingService, configuretionService);
            database.Seed();


        }

        //protected void Application_BeginRequest()
        //{
        //    if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
        //    {
        //        Response.Headers.Add("Access-Control-Allow-Origin", "http://IP:PORT");
        //        Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token");
        //        Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PATCH, PUT, DELETE, OPTIONS");
        //        Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        //        Response.Headers.Add("Access-Control-Max-Age", "1728000");
        //        Response.End();
        //    }
        //}
    }
}
