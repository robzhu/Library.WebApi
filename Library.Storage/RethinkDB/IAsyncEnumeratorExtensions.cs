using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb;

namespace Library.Storage.RethinkDB
{
    public static class IAsyncEnumeratorExtensions
    {
        public static async Task<IEnumerable<T>> FlushAsync<T>( this IAsyncEnumerator<T> enumerator )
        {
            return await FlushToListAsync<T>( enumerator );
        }

        public static async Task<IList<T>> FlushToListAsync<T>( this IAsyncEnumerator<T> enumerator )
        {
            List<T> items = new List<T>();
            while( true )
            {
                if( !await enumerator.MoveNext() )
                {
                    break;
                }
                items.Add( enumerator.Current );
            }
            return items;
        }

        public static async Task VisitAsync<T>( this IAsyncEnumerator<T> enumerator, Action<T> visitor )
        {
            while( await enumerator.MoveNext() )
            {
                visitor( enumerator.Current );
            }
        }
    }
}