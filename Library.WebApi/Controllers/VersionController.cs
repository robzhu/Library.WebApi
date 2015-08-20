using System.Web.Http;
using System.Web.Http.Description;

namespace Library.WebApi
{
    [AllowAnonymous]
    public class VersionController : ApiController
    {
        [ResponseType(typeof(string))]
        public IHttpActionResult Get()
        {
            return Ok( Service.Version );
        }
    }
}
