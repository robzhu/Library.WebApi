using DDD;
using Library.Storage.RethinkDB;

namespace Library.Storage
{
    public static class DocumentStoreFactory
    {
        public const string DbName = "library";

        public static IDocumentStore<T> Create<T>( string tableName ) where T: IEntity
        {
            return new InMemoryDocumentStore<T>();
            //return new RethinkDBDocumentStore<T>( DbName, tableName );
        }
    }
}
