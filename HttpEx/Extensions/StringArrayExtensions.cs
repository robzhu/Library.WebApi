using System.Text;

namespace HttpEx
{
    public static class StringArrayExtensions
    {
        public const string Delimiter = ",";

        public static string ToCommaList( this string[] items )
        {
            StringBuilder sb = new StringBuilder();
            foreach( var item in items )
            {
                if( sb.Length > 1 )
                {
                    sb.Append( Delimiter );
                }
                sb.Append( item );
            }
            return sb.ToString();
        }
    }
}
