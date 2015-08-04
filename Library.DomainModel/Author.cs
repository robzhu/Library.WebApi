using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD;

namespace Library.DomainModel
{
    public class Author : Entity
    {
        public static Author CreateFromFullName( string fullName )
        {
            if( string.IsNullOrEmpty( fullName ) ) throw new ArgumentNullException( "fullName cannot be empty or null" );
            //expect fullName to be in the format "Last, First"
            if( !fullName.Contains( "," ) ) throw new ArgumentException( "fullName must be of the format 'Last, First'" );

            var parts = fullName.Split( ',' );

            var first = parts[ 1 ];
            first = first.TrimStart( ' ' ).TrimEnd( ' ' );

            var last = parts[ 0 ];
            last = last.TrimStart( ' ' ).TrimEnd( ' ' );

            return new Author
            {
                FullName = fullName,
                FirstName = first,
                LastName = last,
            };
        }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> BookIds { get; set; }

        public Author()
        {
            BookIds = new List<string>();
        }

        public async Task AddBookIfNotAlreadyPresentAsync( Book book )
        {
            if( BookIds.Contains( book.Id ) )
            {
                return;
            }
            BookIds.Add( book.Id );
        }
    }
}
