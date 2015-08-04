using System.Web.Http;

namespace HttpEx
{
    public static class ApiControllerExtensions
    {
        public static string GetRequestUriString( this ApiController controller )
        {
            return controller.Request.RequestUri.ToString();
        }

        public static string LinkTo( this ApiController controller, string routeName, object routeValues )
        {
            return controller.Url.Link( routeName, routeValues );
        }
    }
}
