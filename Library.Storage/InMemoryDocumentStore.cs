using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DDD;

namespace Library.Storage
{
    #pragma warning disable 1998
    public class InMemoryDocumentStore<T> : IDocumentStore<T>, IEnumerable<T> where T : IEntity
    {
        private IDictionary<string, T> _items = new Dictionary<string, T>();
        public IDictionary<string, T> Items
        {
            get { return _items; }
        }

        public InMemoryDocumentStore() { }
        public InMemoryDocumentStore( IEnumerable<T> items )
        {
            AddRange( items );
        }

        public async Task CreateAsync( T newItem )
        {
            _items[ newItem.Id ] = newItem;
        }

        public async Task<T> ReadAsync( string id )
        {
            if( _items.ContainsKey( id ) )
            {
                return _items[ id ];
            }
            else return default( T );
        }

        public async Task<IEnumerable<T>> ReadAsync( Expression<Func<T, bool>> filterExpression = null )
        {
            if( filterExpression == null )
            {
                return _items.Values;
            }
            else
            {
                Func<T, bool> func = filterExpression.Compile();
                return _items.Values.Where( item => func( item ) );
            }
        }

        public async Task UpdateAsync( T value )
        {
            _items[ value.Id ] = value;
        }

        public async Task DeleteAsync( string id )
        {
            _items.Remove( id );
        }

        public async Task DeleteAsync( Expression<Func<T, bool>> filterExpression = null )
        {
            if( filterExpression == null )
            {
                _items.Clear();
            }
            else
            {
                Func<T, bool> func = filterExpression.Compile();
                IEnumerable<string> keysToRemove = _items.Keys.Where( key => func( _items[ key ] ) );
                foreach( var key in keysToRemove )
                {
                    _items.Remove( key );
                }
            }
        }

        public async Task<bool> ContainsAsync( string id )
        {
            return _items.ContainsKey( id );
        }

        public async Task<bool> ContainsAsync( T value )
        {
            return _items.ContainsKey( value.Id );
        }

        public async Task<bool> ContainsAsync( Expression<Func<T, bool>> filterExpression )
        {
            Func<T, bool> predicate = filterExpression.Compile();
            return _items.Any( kvp => predicate( kvp.Value ) );
        }

        public void Dispose()
        {
        }

        public void Add( T value )
        {
            if( value != null )
            {
                _items.Add( value.Id, value );
            }
        }

        public void AddRange( IEnumerable<T> range )
        {
            foreach( var item in range )
            {
                Add( item );
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }



        public async Task<IList<T>> OrderByAsync( Expression<Func<T, object>> orderByExpression, int limit = - 1 )
        {
            var values = _items.Values.ToList();
            values.Sort( new AscendingExpressionComparer<T>( orderByExpression ) );

            if( limit < 0 )
            {
                return values;
            }
            else
            {
                return values.Take( limit ).ToList();
            }
        }

        public async Task<IList<T>> OrderByDescendingAsync( Expression<Func<T, object>> orderByExpression, int limit = - 1 )
        {
            var values = _items.Values.ToList();
            values.Sort( new DescendingExpressionComparer<T>( orderByExpression ) );

            if( limit < 0 )
            {
                return values;
            }
            else
            {
                return values.Take( limit ).ToList();
            }
        }
    }
    #pragma warning restore 1998
}
