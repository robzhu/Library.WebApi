using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.DomainModel;
using Library.DomainModel.Storage;

namespace Library.Storage.Providers
{
    public class AuthorDocumentStore : IAuthorStore
    {
        private IDocumentStore<Author> DocumentStore { get; set; }

        public AuthorDocumentStore( IDocumentStore<Author> docStore )
        {
            DocumentStore = docStore;
        }

        public async Task<Author> GetByFullNameAsync( string name )
        {
            return ( await DocumentStore.ReadAsync( b => b.FullName == name ) ).FirstOrDefault();
        }

        public async Task CreateAsync( Author author )
        {
            await DocumentStore.CreateAsync( author );
        }

        public async Task UpdateAsync( Author author )
        {
            await DocumentStore.UpdateAsync( author );
        }

        public async Task<Author> GetByIdAsync( string id )
        {
            return await DocumentStore.ReadAsync( id );
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await DocumentStore.ReadAsync();
        }
    }
}
