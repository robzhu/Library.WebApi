using System.Collections.Generic;
using System.Threading.Tasks;
using Library.DomainModel.Storage;

namespace Library.DomainModel
{
    public interface IAuthorService
    {
        Task<Author> GetAuthorByIdAsync( string id );
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
    }

    public class AuthorService : IAuthorService
    {
        public IAuthorStore AuthorStore { get; set; }

        public AuthorService( IAuthorStore authorStore )
        {
            AuthorStore = authorStore;
        }

        public async Task<Author> GetAuthorByIdAsync( string id )
        {
            return await AuthorStore.GetByIdAsync( id );
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await AuthorStore.GetAllAsync();
        }
    }
}
