using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HttpEx.REST;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    [RoutePrefix( "Book" )]
    public class BookController : ApiController
    {
        public IBookService BookService { get; set; }
        private BookResourceAssembler BookAssembler { get; set; }

        public BookController( IBookService bookService, BookResourceAssembler assembler )
        {
            BookService = bookService;
            BookAssembler = assembler;
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        [ResponseType( typeof( IEnumerable<BookResource> ) )]
        public async Task<IHttpActionResult> GetAsync( string expand = null )
        {
            IEnumerable<Book> books = await BookService.GetAllBooksAsync();
            var resourceCollection = await BookAssembler.ConvertToResourceCollectionAsync( this, books, expand );
            return Ok( resourceCollection );
        }

        /// <summary>
        /// Retrieves the book with the specified id.
        /// </summary>
        /// <param name="id">The Id of the book to retrieve.</param>
        [ResponseType( typeof( BookResource ) )]
        public async Task<IHttpActionResult> GetAsync( string id, string expand = null )
        {
            Book book = await BookService.GetBookByIdAsync( id );
            if( book == null ) return NotFound();

            var resource = await BookAssembler.ConvertToResourceAsync( this, book, expand );
            return Ok( resource );
        }

        public const string Route_Book_GetByAuthorId = "Route_Book_GetByAuthorId";
        /// <summary>
        /// Retrieves all the books written by a specific author
        /// </summary>
        /// <param name="authorId">The Id of the author.</param>
        /// <param name="expand"></param>
        [ResponseType( typeof( ResourceCollection<BookResource> ) )]
        [Route( "ByAuthor/{authorId}", Name = Route_Book_GetByAuthorId )]
        public async Task<IHttpActionResult> GetByAuthorIdAsync( string authorId, string expand = null )
        {
            IEnumerable<Book> books = await BookService.GetBooksByAuthorIdAsync( authorId );
            var resourceCollection = await BookAssembler.ConvertToResourceCollectionAsync( this, books, expand );
            resourceCollection.Href = Request.RequestUri.ToString();

            return Ok( resourceCollection );
        }

        /// <summary>
        /// Adds a new book to the inventory.
        /// </summary>
        /// <param name="isbn">The ISBN of the book to add.</param>
        /// <returns>The book that was added.</returns>
        [HttpPost]
        [ResponseType( typeof( BookResource ) )]
        public async Task<IHttpActionResult> AddBookAsync( string isbn )
        {
            Book book = await BookService.AddByISBNAsync( isbn );

            if( book == null ) return BadRequest( "the provided ISBN does not resolve to a book in our backend service" );

            var resource = await BookAssembler.ConvertToResourceAsync( this, book );
            return CreatedAtRoute( "api", new { id = book.Id }, resource );
        }
    }
}
