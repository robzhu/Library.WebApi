using System.Threading.Tasks;
using Library.DomainModel;

namespace Library.DomainServices
{
    public class ISBNDBLookupService : IISBNLookupService
    {
        private ISBNDB.Driver _isbnService = new ISBNDB.Driver( "IG7T4BLJ" );

        public async Task<string> GetAuthorNameByISBNAsync( string isbn )
        {
            ISBNDB.Book requestedBook = await _isbnService.GetByISBNAsync( isbn );
            return requestedBook.Author_Data[ 0 ].Name;
        }

        public async Task<Book> GetByISBNAsync( string isbn )
        {
            ISBNDB.Book requestedBook = await _isbnService.GetByISBNAsync( isbn );

            return new Book
            {
                Condition = Condition.New,
                IsAvailable = true,
                Title = requestedBook.Title,
                ISBN = isbn,
                AuthorId = requestedBook.Author_Data[ 0 ].Name,
            };
        }
    }
}
