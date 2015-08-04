using Library.DomainModel;

namespace Library.Storage
{
    public static class DocumentStoreIndex
    {
        public static IDocumentStore<Book> BookStore { get; private set; }

        public static IDocumentStore<Author> AuthorStore { get; private set; }

        public static IDocumentStore<LendingRecord> LendingRecordStore { get; private set; }

        static DocumentStoreIndex()
        {
            BookStore = DocumentStoreFactory.Create<Book>( "books" );
            AuthorStore = DocumentStoreFactory.Create<Author>( "authors" );
            LendingRecordStore = DocumentStoreFactory.Create<LendingRecord>( "lendingRecords" );
        }
    }
}
