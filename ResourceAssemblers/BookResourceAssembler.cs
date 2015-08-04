using System;
using System.Threading.Tasks;
using System.Web.Http;
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

        public BookResourceAssembler( IBookService bookService, Lazy<AuthorResourceAssembler> authorAssemblerLazy, LendingRecordResourceAssembler lendingRecordAssembler )
        {
            BookService = bookService;
            _authorAssemblerLazy = authorAssemblerLazy;
            LendingRecordAssembler = lendingRecordAssembler;
        }

        public override async Task<BookResource> ConvertToResourceAsync( ApiController controller, Book model, ExpandQuery expand )
        {
            var resource = new BookResource();

            resource.Href       = GetSingleResourceLink( controller, model.Id );
            resource.Title      = model.Title;
            resource.ISBN       = model.ISBN;
            resource.Published  = model.Published;
            resource.Condition  = model.Condition.ToString();

            if( expand.Contains( () => resource.Author ) ) {
                resource.Author = await AuthorAssembler.GetResourceByIdAsync( controller, model.AuthorId );
            }
            else {
                resource.Author = controller.LinkTo( DefaultRouteName, new { controller = GetPrefix<AuthorController>(), id = model.AuthorId } );
            }

            if( !string.IsNullOrEmpty( model.LendingRecordId ) )
            {
                if( expand.Contains( () => resource.LendingRecord ) ) {
                    resource.LendingRecord = await LendingRecordAssembler.GetResourceByIdAsync( controller, model.LendingRecordId );
                }
                else {
                    resource.LendingRecord = controller.LinkTo( DefaultRouteName, new { controller = GetPrefix<LendingRecordController>(), id = model.LendingRecordId } );
                }
            }

            if( model.IsAvailable ) {
                resource.Checkout = controller.LinkTo( BorrowController.Route_Borrow_Checkout, new { bookId = model.Id } );
            }
            else {
                resource.Checkin = controller.LinkTo( BorrowController.Route_Borrow_Checkin, new { bookId = model.Id } );
            }

            return resource;
        }

        public async Task<BookResource> GetResourceWithIdAsync( ApiController controller, string id )
        {
            var model = await BookService.GetBookByIdAsync( id );
            return await ConvertToResourceAsync( controller, model );
        }

        public string GetSingleResourceLink( ApiController controller, string modelId )
        {
            return controller.LinkTo( DefaultRouteName, new { controller = GetPrefix<BookController>(), id = modelId } );
        }
    }
}
