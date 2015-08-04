
namespace ISBNDB
{
    public class BookResponse
    {
        public string Index_searched { get; set; }
        public Book[] Data { get; set; }
    }

    public struct Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Book
    {
        public string Summary { get; set; }
        public string Language { get; set; }
        public string ISBN10 { get; set; }
        public string Title { get; set; }
        public Author[] Author_Data { get; set; }
    }
}
