using System.IO;
using System.Reflection;
using System.Web.Http;
using HttpEx;
using Owin;
using Swashbuckle.Application;
 
namespace Library.WebApi
{
    public class Startup
    {
        public const string DefaultRouteName = "api";

        public void Configuration( IAppBuilder app )
        {
            var config = new HttpConfiguration();

            IoCSettings.Configure( app, config );

            JsonUtility.ConfigureDefaults();
            config.ConfigureJsonMediaTypeFormatter();

            config.Routes.MapHttpRoute( DefaultRouteName, "{controller}/{id}", defaults: new { controller = "Root", id = RouteParameter.Optional } );
            //config.MapHttpAttributeRoutes();

            var execPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

            config.EnableSwagger( c =>
            {
                c.IncludeXmlComments( execPath + "\\docs.xml" );
                c.SingleApiVersion( "0.1", "REST API/MVC Demo: Public Library" );
            } ).EnableSwaggerUi( c =>
            {
                c.DisableValidator();
            } );
 
            app.UseWebApi( config );
        }
    }
}