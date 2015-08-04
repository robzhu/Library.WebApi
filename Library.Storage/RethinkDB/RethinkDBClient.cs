using System;
using System.Linq;
using System.Threading.Tasks;
using DDD;
using Newtonsoft.Json.Converters;
using RethinkDb;
using RethinkDb.Newtonsoft.Configuration;

namespace Library.Storage.RethinkDB
{
    public class RethinkDBClient : IDisposable
    {
        public const string RethinkDBClusterName = "library";

        //lazy cache for the default connection.
        public static AsyncLazy<IConnection> DefaultConnectionCache { get; set; }

        static RethinkDBClient()
        {
            //by default convert all enums to strings (or else config assembler will default to int32).
            RethinkDb.Newtonsoft.Configuration.ConfigurationAssembler.DefaultJsonSerializerSettings.Converters.Add( new StringEnumConverter() );

            DefaultConnectionCache = new AsyncLazy<IConnection>( async () =>
            {
                return await ConfigurationAssembler.CreateConnectionFactory( RethinkDBClusterName ).GetAsync();
            } );
        }

        public IConnection Connection { get; protected set; }

        public async Task InitConnectionAsync()
        {
            if( Connection == null )
            {
                Connection = await DefaultConnectionCache;
            }
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            if( disposing && Connection != null )
            {
                Connection.Dispose();
            }
        }
    }

    public class RethinkDBClient<T> : RethinkDBClient
    {
        public IDatabaseQuery Database{ get; protected set; }
        public ITableQuery<T> Table { get; protected set; }

        public string DbName { get; private set; }
        public string TableName { get; private set; }

        public bool _verifiedTableExists = false;

        public RethinkDBClient( string dbName, string tableName )
        {
            DbName = dbName;
            TableName = tableName;
        }

        public RethinkDBClient( string dbName, IConnection connection ) : this( dbName, typeof( T ).Name.ToLowerInvariant(), connection ) { }
        public RethinkDBClient( string dbName, string tableName, IConnection connection )
        {
            DbName = dbName;
            TableName = tableName;
            Connection = connection;
        }

        public async Task EnsureDbAndTableExistOrCreateAsync()
        {
            if( _verifiedTableExists ) return;
            await InitConnectionAsync();

            string[] dbs = await Connection.RunAsync( Query.DbList() );
            if( !dbs.Contains( DbName ) )
            {
                //TODO: log this
                await Connection.RunAsync( Query.DbCreate( DbName ) );
            }
            Database = Query.Db( DbName );

            var tables = await Connection.RunAsync( Database.TableList() );
            if( !tables.Contains( TableName ) )
            {
                //TODO: log
                await Connection.RunAsync( Database.TableCreate( TableName ) );
            }

            Table = Database.Table<T>( TableName );
            _verifiedTableExists = true;
        }

        protected async Task<TOut> RunAsync<TOut>( IScalarQuery<TOut> queryObject )
        {
            return await Connection.RunAsync( queryObject );
        }

        protected IAsyncEnumerator<TOut> RunAsync<TOut>( ISequenceQuery<TOut> queryObject, IQueryConverter queryConverter = null )
        {
            return Connection.RunAsync( queryObject, queryConverter );
        }
    }
}