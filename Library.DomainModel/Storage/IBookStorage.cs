using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.DomainModel.Storage
{
    public interface IBookStorage
    {
        Task WriteBookAsync( Book book );
        Task<Book> ReadByIdAsync( string id );
        
        Task UpdateAsync( Book book );

        Task<IEnumerable<Book>> ReadAllAsync();
        Task<IEnumerable<Book>> BatchReadByIdsAsync( string[] ids );
        Task<IEnumerable<Book>> ReadByAuthorIdAsync( string authorId );
    }
}
