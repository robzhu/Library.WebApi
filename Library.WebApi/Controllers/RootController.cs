using System.Web.Http;
using System.Web.Http.Description;

namespace Library.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RootController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Redirect( Request.RequestUri.AbsoluteUri + "swagger/ui/index" );
        }
    }
}
