using System;
using HttpEx.REST;
using Newtonsoft.Json;

namespace Library.DataTransferObjects
{
    public enum BookResourceCondition
    {
        New,
        Good,
        Used,
        Damaged,
    }

    public class BookResource : Resource
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateTime Published { get; set; }
        public Hyperlink<AuthorResource> Author { get; set; }
        public string Condition { get; set; }
        public Hyperlink<LendingRecordResource> LendingRecord { get; set; }

        [JsonProperty( NullValueHandling = NullValueHandling.Ignore )]
        public Actionlink Checkout { get; set; }

        [JsonProperty( NullValueHandling = NullValueHandling.Ignore )]
        public Actionlink Checkin { get; set; }
    }
}
