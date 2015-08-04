using System.Diagnostics;

namespace HttpEx.REST
{
    /// <summary>
    /// An actionlink is an RPC style href via POST
    /// </summary>
    [DebuggerDisplay( "{Href}" )]
    public sealed class Actionlink
    {
        public static implicit operator Actionlink( string href )
        {
            return new Actionlink( href );
        }

        public string Href { get; set; }

        public Actionlink() { }
        public Actionlink( string href )
        {
            Href = href;
        }
    }
}
