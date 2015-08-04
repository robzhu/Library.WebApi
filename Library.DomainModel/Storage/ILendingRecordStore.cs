using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.DomainModel.Storage
{
    public interface ILendingRecordStore
    {
        Task CreateAsync( LendingRecord record );
        Task UpdateAsync( LendingRecord record );

        Task<LendingRecord> GetRecordWithBookIdAsync( string bookId );
        Task<LendingRecord> GetByIdAsync( string id );
        Task<IEnumerable<LendingRecord>> GetAllAsync();
    }
}
