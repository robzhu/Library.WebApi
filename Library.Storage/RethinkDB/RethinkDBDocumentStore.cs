using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RethinkDb;
using DDD;

namespace Library.Storage.RethinkDB
{
    public class RethinkDBDocumentStore<T> : RethinkDBClient<T>, IDocumentStore<T> where T : IEntity
    {
        public RethinkDBDocumentStore( string dbName, string tableName ) : base( dbName, tableName ) { }
        public RethinkDBDocumentStore( string dbName, IConnection connection ) : base( dbName, connection ) { }
        public RethinkDBDocumentStore( string dbName, string tableName, IConnection connection ) : base( dbName, tableName, connection ) { }

        public async Task CreateAsync( T value )
        {
            Debug.Assert( !string.IsNullOrEmpty( value.Id ) );
            await EnsureDbAndTableExistOrCreateAsync();
            await RunAsync( Table.Insert( value ) );
        }

        public async Task<IEnumerable<T>> ReadAsync( Expression<Func<T, bool>> filterExpression = null )
        {
            if( filterExpression == null )
            {
                filterExpression = entity => true;
            }

            await EnsureDbAndTableExistOrCreateAsync();
            IAsyncEnumerator<T> enumerable = Connection.RunAsync( Table.Filter( filterExpression ) );
            return await enumerable.FlushAsync();
        }

        public async Task<T> ReadAsync( string id )
        {
            IEnumerable<T> results = await ReadAsync( entity => entity.Id == id );
            return results.FirstOrDefault();
        }

        public async Task<IList<T>> OrderByAsync( Expression<Func<T, object>> orderByExpression, int limit = - 1 )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            if( limit < 0 )
            {
                return await RunAsync( Table.OrderBy( orderByExpression ) ).FlushToListAsync();
            }
            else
            {
                return await RunAsync( Table.OrderBy( orderByExpression ).Limit( limit ) ).FlushToListAsync();
            }
        }

        public async Task<IList<T>> OrderByDescendingAsync( Expression<Func<T, object>> orderByExpression, int limit = - 1 )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            if( limit < 0 )
            {
                return await RunAsync( Table.OrderByDescending( orderByExpression ) ).FlushToListAsync();
            }
            else
            {
                return await RunAsync( Table.OrderByDescending( orderByExpression ).Limit( limit ) ).FlushToListAsync();
            }
        }

        public async Task UpdateAsync( T value )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            await RunAsync( Table.Update( r => value ) );
        }

        public async Task DeleteAsync( string id )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            await RunAsync( Table.Filter( entity => entity.Id == id ).Delete() );
        }

        public async Task DeleteAsync( Expression<Func<T, bool>> filterExpression = null )
        {
            if( filterExpression == null )
            {
                filterExpression = entity => true;
            }

            await EnsureDbAndTableExistOrCreateAsync();
            await RunAsync( Table.Filter( filterExpression ).Delete() );
        }

        public async Task<bool> ContainsAsync( T value )
        {
            return await ContainsAsync( value.Id );
        }

        public async Task<bool> ContainsAsync( string id )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            return await RunAsync( Table.Contains<T>( e => e.Id == id ) );
        }

        public async Task<bool> ContainsAsync( Expression<Func<T, bool>> filterExpression )
        {
            await EnsureDbAndTableExistOrCreateAsync();
            return await RunAsync( Table.Contains<T>( filterExpression ) );
        }
    }
}