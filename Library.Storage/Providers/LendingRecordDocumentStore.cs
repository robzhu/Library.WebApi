using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.DomainModel;
using Library.DomainModel.Storage;

namespace Library.Storage.Providers
{
    public class LendingRecordDocumentStore : ILendingRecordStore
    {
        private IDocumentStore<LendingRecord> DocumentStore { get; set; }

        public LendingRecordDocumentStore( IDocumentStore<LendingRecord> store )
        {
            DocumentStore = store;
        }

        public async Task CreateAsync( LendingRecord record )
        {
            await DocumentStore.CreateAsync( record );
        }

        public async Task<LendingRecord> GetByIdAsync( string id )
        {
            return await DocumentStore.ReadAsync( id );
        }

        public async Task UpdateAsync( LendingRecord record )
        {
            await DocumentStore.UpdateAsync( record );
        }

        public async Task<IEnumerable<LendingRecord>> GetAllAsync()
        {
            return await DocumentStore.ReadAsync();
        }

        public async Task<LendingRecord> GetRecordWithBookIdAsync( string bookId )
        {
            return ( await DocumentStore.ReadAsync( record => record.BookId == bookId ) ).FirstOrDefault();
        }
    }
}
