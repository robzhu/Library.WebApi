using System.Threading.Tasks;

namespace Library.DomainModel
{
    public interface IISBNLookupService
    {
        Task<string> GetAuthorNameByISBNAsync( string isbn );
        Task<Book> GetByISBNAsync( string isbn );
    }
}
