using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HttpEx.REST;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    [RoutePrefix("Author")]
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
        [Route( Name = "Author_GetAll" )]
        [ResponseType( typeof( ResourceCollection<AuthorResource> ) )]
        public async Task<IHttpActionResult> GetAllAsync( string expand = null )
        {
            IEnumerable<Author> authors = await AuthorService.GetAllAuthorsAsync();

            var resourceCollection = await AuthorAssembler.ConvertToResourceCollectionAsync( this, authors, expand );
            return Ok( resourceCollection );
        }

        /// <summary>
        /// Retrieves the author with the specified id.
        /// </summary>
        [Route( "{id}", Name = "Author_GetByIdAsync" )]
        [ResponseType( typeof( AuthorResource ) )]
        public async Task<IHttpActionResult> GetByIdAsync( string id, string expand = null )
        {
            var author = await AuthorService.GetAuthorByIdAsync( id );
            if( author == null ) return NotFound();

            var resource = await AuthorAssembler.ConvertToResourceAsync( this, author, expand );
            return Ok( resource );
        }
    }
}
