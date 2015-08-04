using System.Diagnostics;
using Newtonsoft.Json;

namespace HttpEx.REST
{
    public interface IHyperlink : IResource
    {
        bool IsLinkOnly { get; }
        object ObjectValue { get; }
    }

    [DebuggerDisplay( "{Href}" )]
    public class Hyperlink<T> : IHyperlink where T : IResource
    {
        public static implicit operator Hyperlink<T>( string href )
        {
            return new Hyperlink<T>() { Href = href };
        }

        public static implicit operator Hyperlink<T>( T value )
        {
            return new Hyperlink<T>() { Value = value };
        }

        private string _href;
        public string Href
        {
            get
            {
                return ( Value == null ) ? _href : Value.Href;
            }
            set
            {
                if( Value == null )
                {
                    _href = value;
                }
                else
                {
                    Value.Href = value;
                }
            }
        }

        [JsonIgnore]
        public T Value { get; set; }

        [JsonIgnore]
        public object ObjectValue
        {
            get { return Value; }
        }

        /// <summary>
        /// Gets whether this resource is a hyperlink only. 
        /// If true, serialization of this resource should not include any properties other than Href.
        /// </summary>
        [JsonIgnore]
        public bool IsLinkOnly { get { return Value == null; } }

        public Hyperlink() { }
        public Hyperlink( T value )
        {
            Value = value;
        }
    }
}
