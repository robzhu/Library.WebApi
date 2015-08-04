using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.DomainModel.Storage
{
    public interface IAuthorStore
    {
        Task<Author> GetByFullNameAsync( string fullName );
        Task<Author> GetByIdAsync( string id );
        Task<IEnumerable<Author>> GetAllAsync();

        Task CreateAsync( Author author );
        Task UpdateAsync( Author author );
    }
}
