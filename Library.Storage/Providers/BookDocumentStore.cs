using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.DomainModel;
using Library.DomainModel.Storage;

namespace Library.Storage.Providers
{
    public class BookDocumentStore : IBookStorage
    {
        private IDocumentStore<Book> DocumentStore { get; set; }

        public BookDocumentStore( IDocumentStore<Book> docStore )
        {
            DocumentStore = docStore;
        }

        public async Task WriteBookAsync( Book book )
        {
            await DocumentStore.CreateAsync( book );
        }

        public async Task<Book> ReadByIdAsync( string id )
        {
            return await DocumentStore.ReadAsync( id );
        }

        public async Task<IEnumerable<Book>> ReadAllAsync()
        {
            return await DocumentStore.ReadAsync();
        }

        public async Task UpdateAsync( Book book )
        {
            await DocumentStore.UpdateAsync( book );
        }

        public Task<IEnumerable<Book>> BatchReadByIdsAsync( string[] ids )
        {
            return DocumentStore.ReadAsync( book => ids.Contains( book.Id ) );
        }

        public Task<IEnumerable<Book>> ReadByAuthorIdAsync( string authorId )
        {
            return DocumentStore.ReadAsync( book => book.AuthorId == authorId );
        }
    }
}
