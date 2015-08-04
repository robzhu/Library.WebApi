using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.DomainModel.Storage;

namespace Library.DomainModel
{
    public interface ILendingService
    {
        Task<LendingRecord> CheckoutAsync( string bookId );
        Task<LendingRecord> CheckinAsync( string bookId );
        Task<IEnumerable<LendingRecord>> GetAllRecordsAsync();
        Task<LendingRecord> GetByIdAsync( string id );
    }

    public class LendingService : ILendingService
    {
        public IBookService BookService { get; set; }
        public ILendingRecordStore LendingRecordStore { get; set; }

        public LendingService( IBookService bookService, ILendingRecordStore lendingRecordStore )
        {
            BookService = bookService;
            LendingRecordStore = lendingRecordStore;
        }

        public async Task<LendingRecord> CheckoutAsync( string bookId )
        {
            if( string.IsNullOrWhiteSpace( bookId ) ) throw new ArgumentException( "bookId cannot be null" );

            var book = await BookService.GetBookByIdAsync( bookId );
            if( book == null ) throw new DomainOperationException( string.Format( "the book with id \"{0}\" could not be found.", bookId ) );
            if( !book.IsAvailable ) throw new DomainOperationException( "that book is not available for checkout" );
            
            var lendingRecord = LendingRecord.Create( book.Id, "asdf" );
            book.Checkout( lendingRecord );

            await LendingRecordStore.CreateAsync( lendingRecord );
            await BookService.UpdateBookAsync( book );
            return lendingRecord;
        }

        public async Task<LendingRecord> CheckinAsync( string bookId )
        {
            LendingRecord record = await LendingRecordStore.GetRecordWithBookIdAsync( bookId );
            if( record == null ) throw new DomainOperationException( "this book could not be checked in because is it not currently on loan." );

            var book = await BookService.GetBookByIdAsync( bookId );
            book.Checkin();
            record.Checkin();

            await BookService.UpdateBookAsync( book );
            await LendingRecordStore.UpdateAsync( record );
            return record;
        }

        public async Task<IEnumerable<LendingRecord>> GetAllRecordsAsync()
        {
            return await LendingRecordStore.GetAllAsync();
        }

        public async Task<LendingRecord> GetByIdAsync( string id )
        {
            return await LendingRecordStore.GetByIdAsync( id );
        }
    }
}
