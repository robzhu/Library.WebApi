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
        [Route( Name = "Book_GetAll" )]
        [ResponseType( typeof( IEnumerable<BookResource> ) )]
        public async Task<IHttpActionResult> GetAllAsync( string expand = null )
        {
            IEnumerable<Book> books = await BookService.GetAllBooksAsync();
            var resourceCollection = await BookAssembler.ConvertToResourceCollectionAsync( books, expand );
            return Ok( resourceCollection );
        }

        /// <summary>
        /// Retrieves the book with the specified id.
        /// </summary>
        /// <param name="id">The Id of the book to retrieve.</param>
        [Route( "{id}", Name = "Book_GetById" )]
        [ResponseType( typeof( BookResource ) )]
        public async Task<IHttpActionResult> GetByIdAsync( string id, string expand = null )
        {
            Book book = await BookService.GetBookByIdAsync( id );
            if( book == null ) return NotFound();

            var resource = await BookAssembler.ConvertToResourceAsync( book, expand );
            return Ok( resource );
        }

        /// <summary>
        /// Retrieves all the books written by a specific author
        /// </summary>
        /// <param name="authorId">The Id of the author.</param>
        /// <param name="expand">The child resources that should be expanded</param>
        [Route( "ByAuthor/{authorId}", Name = "Book_GetByAuthorId" )]
        [ResponseType( typeof( ResourceCollection<BookResource> ) )]
        public async Task<IHttpActionResult> GetByAuthorIdAsync( string authorId, string expand = null )
        {
            IEnumerable<Book> books = await BookService.GetBooksByAuthorIdAsync( authorId );
            var resourceCollection = await BookAssembler.ConvertToResourceCollectionAsync( books, expand );
            resourceCollection.Href = Request.RequestUri.ToString();

            return Ok( resourceCollection );
        }

        /// <summary>
        /// Adds a new book to the inventory.
        /// </summary>
        /// <param name="isbn">The ISBN of the book to add.</param>
        /// <returns>The book that was added.</returns>
        [HttpPost, Route( Name = "Book_AddByIsbn" )]
        [ResponseType( typeof( BookResource ) )]
        public async Task<IHttpActionResult> AddBookAsync( string isbn )
        {
            Book book = await BookService.AddByISBNAsync( isbn );

            if( book == null ) return BadRequest( "the provided ISBN does not resolve to a book in our backend service" );

            var resource = await BookAssembler.ConvertToResourceAsync( book );
            return CreatedAtRoute( "api", new { id = book.Id }, resource );
        }
    }
}
