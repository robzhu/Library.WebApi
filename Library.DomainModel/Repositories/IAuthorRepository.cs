using System.Threading.Tasks;

namespace Library.DomainModel.Storage
{
    public interface IAuthorRepository
    {
        Task<Author> CreateOrGetByNameAsync( string name );
        Task UpdateAuthorAsync( Author author );
    }

    public class AuthorRepository : IAuthorRepository
    {
        private IAuthorStore AuthorStore { get; set; }

        public async Task<Author> CreateOrGetByNameAsync( string name )
        {
            Author existingAuthor = await AuthorStore.GetByFullNameAsync( name );
            if( existingAuthor != null )
            {
                return existingAuthor;
            }
            
            //no such author exists, create it, and then store it.
            var author = Author.CreateFromFullName( name );
            await AuthorStore.CreateAsync( author );
            return author;
        }

        public async Task UpdateAuthorAsync( Author author )
        {
            await AuthorStore.UpdateAsync( author );
        }
    }
}
