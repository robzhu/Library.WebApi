using System.Diagnostics;
using Newtonsoft.Json;

namespace HttpEx.REST
{
    public interface IResource
    {
        string Href { get; set; }
    }

    public abstract class Resource : IResource
    {
        /// <summary>
        /// The "self" hypermedia link
        /// </summary>
        [JsonProperty( Order = -100 )]
        public string Href { get; set; }
    }
}