using System;
using System.Linq.Expressions;

namespace HttpEx
{
    /// <summary>
    /// An expand query is a query string that indicates which of the resource's links should be expanded inline
    /// </summary>
    /// <example>
    /// http://site.com/api/book/1234?expand=author,publisher
    /// Tells the api to return a book resource with the author and publisher resource links expanded inline
    /// </example>
    public class ExpandQuery
    {
        public static implicit operator ExpandQuery( string value )
        {
            if( string.IsNullOrEmpty( value ) )
            {
                return Default;
            }
            return new ExpandQuery( value );
        }

        private static ExpandQuery _default = new ExpandQuery( string.Empty );
        public static ExpandQuery Default
        {
            get { return _default; }
        }

        public string Value { get; private set; }

        public ExpandQuery( string value )
        {
            Value = value.ToLowerInvariant();
        }

        public bool Contains<T>( Expression<Func<T>> func )
        {
            var name = ( (MemberExpression)func.Body ).Member.Name;
            return Value.Contains( name.ToLowerInvariant() );
        }
    }
}
