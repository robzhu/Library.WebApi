using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HttpEx.REST;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    public class AuthorController : ApiController
    {
        private IAuthorService AuthorService { get; set; }
        private AuthorResourceAssembler AuthorAssembler { get; set; }

        public AuthorController( IAuthorService authorService, AuthorResourceAssembler assembler )
        {
            AuthorService = authorService;
            AuthorAssembler = assembler;
        }

        /// <summary>
        /// Retrieves all the authors in the system.
        /// </summary>
        [ResponseType( typeof( ResourceCollection<AuthorResource> ) )]
        public async Task<IHttpActionResult> GetAsync( string expand = null )
        {
            IEnumerable<Author> authors = await AuthorService.GetAllAuthorsAsync();

            var resourceCollection = await AuthorAssembler.ConvertToResourceCollectionAsync( this, authors, expand );
            return Ok( resourceCollection );
        }

        /// <summary>
        /// Retrieves the author with the specified id.
        /// </summary>
        [ResponseType( typeof( AuthorResource ) )]
        public async Task<IHttpActionResult> GetAsync( string id, string expand = null )
        {
            var author = await AuthorService.GetAuthorByIdAsync( id );
            if( author == null ) return NotFound();

            var resource = await AuthorAssembler.ConvertToResourceAsync( this, author, expand );
            return Ok( resource );
        }
    }
}
