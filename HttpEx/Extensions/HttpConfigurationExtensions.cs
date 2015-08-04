using System.Net.Http.Formatting;
using System.Web.Http;

namespace HttpEx
{
    public static class HttpConfigurationExtensions
    {
        public static void ConfigureJsonMediaTypeFormatter( this HttpConfiguration config )
        {
            config.Formatters.Clear();
            config.Formatters.Add( new JsonMediaTypeFormatter() );
            config.Formatters.JsonFormatter.SerializerSettings = JsonUtility.DefaultSettings;
        }
    }
}
