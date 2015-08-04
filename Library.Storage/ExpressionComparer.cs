using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Library.Storage
{
    /// <summary>
    /// Comparer for generic type T using a delegate that finds the member on which to compare.
    /// </summary>
    public class AscendingExpressionComparer<T> : IComparer<T>
    {
        private Func<T, object> _memberFunc;

        public AscendingExpressionComparer( Expression<Func<T, object>> memberExpression )
            : this( memberExpression.Compile() )
        {
        }

        public AscendingExpressionComparer( Func<T, object> memberFunc )
        {
            _memberFunc = memberFunc;
        }

        public int Compare( T x, T y )
        {
            IComparable memberOfX = _memberFunc( x ) as IComparable;

            if( memberOfX != null )
            {
                object memberOfY = _memberFunc( y );
                return memberOfX.CompareTo( memberOfY );
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Comparer for generic type T using a delegate that finds the member on which to compare.
    /// </summary>
    public class DescendingExpressionComparer<T> : IComparer<T>
    {
        private Func<T, object> _memberFunc;

        public DescendingExpressionComparer( Expression<Func<T, object>> memberExpression )
            : this( memberExpression.Compile() )
        {
        }

        public DescendingExpressionComparer( Func<T, object> memberFunc )
        {
            _memberFunc = memberFunc;
        }

        public int Compare( T x, T y )
        {
            IComparable memberOfX = _memberFunc( x ) as IComparable;

            if( memberOfX != null )
            {
                object memberOfY = _memberFunc( y );
                return -memberOfX.CompareTo( memberOfY );
            }
            else
            {
                return 0;
            }
        }
    }
}
