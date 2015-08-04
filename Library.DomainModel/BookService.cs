using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.DomainModel.Storage;

namespace Library.DomainModel
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetBooksByIdsAsync( string[] ids );
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync( string authorId );

        Task<Book> AddByISBNAsync( string isbn );
        Task<Book> GetBookByIdAsync( string id );
        Task<Book> TrySetUnavailableAsync( string id );
        Task<Book> TrySetAvailableAsync( string id );

        Task UpdateBookAsync( Book book );
    }

    public class BookService : IBookService
    {
        public IBookStorage BookStorage { get; set; }
        public IAuthorRepository AuthorRepository { get; set; }
        public IISBNLookupService ISBNService { get; set; }

        public BookService( IBookStorage bookStorage, IAuthorRepository authorRepo, IISBNLookupService isbnLookupService )
        {
            BookStorage = bookStorage;
            AuthorRepository = authorRepo;
            ISBNService = isbnLookupService;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await BookStorage.ReadAllAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByIdsAsync( string[] ids )
        {
            return await BookStorage.BatchReadByIdsAsync( ids );
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync( string authorId )
        {
            return await BookStorage.ReadByAuthorIdAsync( authorId );
        }

        public async Task<Book> AddByISBNAsync( string isbn )
        {
            //check if an author exists for this ISBN
            var authorName = await ISBNService.GetAuthorNameByISBNAsync( isbn );
            var author = await AuthorRepository.CreateOrGetByNameAsync( authorName );

            var book = await ISBNService.GetByISBNAsync( isbn );
            book.SetAuthor( author );

            //TODO: this should be represented in a unit of work
            await author.AddBookIfNotAlreadyPresentAsync( book );

            //commit all the changes
            await AuthorRepository.UpdateAuthorAsync( author );
            await BookStorage.WriteBookAsync( book );

            return book;
        }

        public async Task<Book> GetBookByIdAsync( string id )
        {
            return await BookStorage.ReadByIdAsync( id );
        }

        public async Task<Book> TrySetUnavailableAsync( string id )
        {
            var book = await BookStorage.ReadByIdAsync( id );
            if( !book.IsAvailable )
            {
                throw new InvalidOperationException( "that book is not available." );
            }

            book.IsAvailable = false;
            await BookStorage.UpdateAsync( book );
            return book;
        }

        public async Task<Book> TrySetAvailableAsync( string id )
        {
            var book = await BookStorage.ReadByIdAsync( id );
            if( book.IsAvailable )
            {
                throw new InvalidOperationException( "that book is already available." );
            }

            book.IsAvailable = true;
            await BookStorage.UpdateAsync( book );
            return book;
        }

        public async Task UpdateBookAsync( Book book )
        {
            await BookStorage.UpdateAsync( book );
        }
    }
}
