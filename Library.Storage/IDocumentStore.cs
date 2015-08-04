using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DDD;

namespace Library.Storage
{
    public interface IDocumentStore<T> : IDisposable where T : IEntity
    {
        Task CreateAsync( T newItem );
        Task<T> ReadAsync( string id );
        Task<IEnumerable<T>> ReadAsync( Expression<Func<T, bool>> filterExpression = null );
        Task UpdateAsync( T value );
        Task DeleteAsync( string id );
        Task DeleteAsync( Expression<Func<T, bool>> filterExpression = null );
        Task<bool> ContainsAsync( string id );
        Task<bool> ContainsAsync( T value );
        Task<bool> ContainsAsync( Expression<Func<T, bool>> filterExpression );
    }
}
