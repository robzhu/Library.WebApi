using System;
using System.Threading.Tasks;
using HttpEx;
using Library.DataTransferObjects;
using Library.DomainModel;
using Library.DomainModel.Storage;

namespace Library.WebApi
{
    public class LendingRecordResourceAssembler : ResourceAssembler<LendingRecord, LendingRecordResource>
    {
        Lazy<BookResourceAssembler> _assemblerLazy;
        BookResourceAssembler BookAssembler { get { return _assemblerLazy.Value; } }
        ILendingRecordStore LendingRecordStore { get; set; }
        IBookService BookService { get; set; }

        public LendingRecordResourceAssembler( Lazy<BookResourceAssembler> assemblerLazy, ILendingRecordStore lendingRecordStore, IBookService bookService, IUrlProvider urlProvider ) 
            : base( urlProvider )
        {
            _assemblerLazy = assemblerLazy;
            LendingRecordStore = lendingRecordStore;
            BookService = bookService;
        }

        public override async Task<LendingRecordResource> ConvertToResourceAsync( LendingRecord model, ExpandQuery expand )
        {
            var resource = new LendingRecordResource();

            resource.Href           = UrlProvider.UriStringFor<LendingRecordController>( c => c.GetByIdAsync( model.Id, expand.Value ) );
            resource.CheckoutDate   = model.CheckoutDate;
            resource.DueDate        = model.DueDate;
            resource.ReturnedDate   = model.ReturnedDate;
            resource.Span           = model.Span;
            resource.UserId         = model.UserId;
            resource.State          = model.State.ToString();

            if( expand.Contains( () => resource.Book ) ) {
                var book = await BookService.GetBookByIdAsync( model.BookId );
                resource.Book = await BookAssembler.ConvertToResourceAsync(  book );
            }
            else {
                resource.Book = UrlProvider.UriStringFor<BookController>( c => c.GetByIdAsync( model.BookId, null ) );
            }

            return resource;
        }

        internal async Task<LendingRecordResource> GetResourceByIdAsync( string id )
        {
            var record = await LendingRecordStore.GetByIdAsync( id );
            return await ConvertToResourceAsync( record );
        }
    }
}
