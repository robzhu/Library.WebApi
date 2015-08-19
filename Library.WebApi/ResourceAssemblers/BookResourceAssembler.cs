using System;
using System.Threading.Tasks;
using HttpEx;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    public class BookResourceAssembler : ResourceAssembler<Book, BookResource>
    {
        IBookService BookService { get; set; }

        Lazy<AuthorResourceAssembler> _authorAssemblerLazy;
        AuthorResourceAssembler AuthorAssembler { get { return _authorAssemblerLazy.Value; } }
        
        LendingRecordResourceAssembler LendingRecordAssembler { get; set; }

        public BookResourceAssembler( IBookService bookService, 
                                      Lazy<AuthorResourceAssembler> authorAssemblerLazy, 
                                      LendingRecordResourceAssembler lendingRecordAssembler, 
                                      IUrlProvider urlProvider )
            : base( urlProvider )
        {
            BookService = bookService;
            _authorAssemblerLazy = authorAssemblerLazy;
            LendingRecordAssembler = lendingRecordAssembler;
        }

        public override async Task<BookResource> ConvertToResourceAsync( Book model, ExpandQuery expand )
        {
            var resource = new BookResource();

            resource.Href       = GetSingleResourceLink( model.Id );
            resource.Title      = model.Title;
            resource.ISBN       = model.ISBN;
            resource.Published  = model.Published;
            resource.Condition  = model.Condition.ToString();

            if( expand.Contains( () => resource.Author ) ) {
                resource.Author = await AuthorAssembler.GetResourceByIdAsync( model.AuthorId );
            }
            else {
                resource.Author = UrlProvider.UriStringFor<AuthorController>( c => c.GetByIdAsync( model.AuthorId, null ) );
            }

            if( !string.IsNullOrEmpty( model.LendingRecordId ) )
            {
                if( expand.Contains( () => resource.LendingRecord ) ) {
                    resource.LendingRecord = await LendingRecordAssembler.GetResourceByIdAsync( model.LendingRecordId );
                }
                else {
                    resource.LendingRecord = UrlProvider.UriStringFor<LendingRecordController>( c => c.GetByIdAsync( model.LendingRecordId, null ) );
                }
            }

            if( model.IsAvailable ) {
                resource.Checkout = UrlProvider.UriStringFor<BorrowController>( c => c.CheckoutAsync( model.Id ) );
            }
            else {
                resource.Checkin = UrlProvider.UriStringFor<BorrowController>( c => c.CheckinAsync( model.Id ) );
            }

            return resource;
        }

        public async Task<BookResource> GetResourceWithIdAsync( string id )
        {
            var model = await BookService.GetBookByIdAsync( id );
            return await ConvertToResourceAsync( model );
        }

        public string GetSingleResourceLink( string id )
        {
            return UrlProvider.UriStringFor<BookController>( c => c.GetByIdAsync( id, null ) );
        }
    }
}
