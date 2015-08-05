using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HttpEx;
using HttpEx.REST;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    public class AuthorResourceAssembler : ResourceAssembler<Author, AuthorResource>
    {
        IAuthorService AuthorService { get; set; }
        IBookService BookService { get; set; }
        Lazy<BookResourceAssembler> LazyBookAssembler;
        IUrlProvider UrlProvider;

        BookResourceAssembler BookAssembler 
        {
            get { return LazyBookAssembler.Value; }
        }

        public AuthorResourceAssembler( IAuthorService authorService, IBookService bookService, Lazy<BookResourceAssembler> assemblerLazy, IUrlProvider urlProvider )
        {
            AuthorService = authorService;
            BookService = bookService;
            LazyBookAssembler = assemblerLazy;
            UrlProvider = urlProvider;
        }

        public override async Task<AuthorResource> ConvertToResourceAsync( ApiController controller, Author model, ExpandQuery expand )
        {
            var resource = new AuthorResource();

            resource.Href       = UrlProvider.UriStringFor<AuthorController>( c => c.GetByIdAsync( model.Id, expand.Value ) );
            resource.FirstName  = model.FirstName;
            resource.LastName   = model.LastName;

            IEnumerable<Hyperlink<BookResource>> booksCollection;
            if( expand.Contains( () => resource.Books ) )
            {
                var books = await BookService.GetBooksByIdsAsync( model.BookIds.ToArray() );
                booksCollection = await BookAssembler.ConvertToResourceEnumerableAsync( controller, books );
            }
            else
            {
                //unexpanded hyperlinks
                var linksCollection = new List<Hyperlink<BookResource>>();
                foreach( var bookId in model.BookIds )
                {
                    string href = BookAssembler.GetSingleResourceLink( controller, bookId );
                    linksCollection.Add( href );
                }
                booksCollection = linksCollection;
            }
            resource.Books = booksCollection;

            return resource;
        }

        internal async Task<AuthorResource> GetResourceByIdAsync( ApiController controller, string id )
        {
            Author author = await AuthorService.GetAuthorByIdAsync( id );
            if( author == null ) return null;

            return await ConvertToResourceAsync( controller, author );
        }
    }
}
