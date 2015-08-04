using System;
using HttpEx.REST;

namespace Library.DataTransferObjects
{
    public class LendingRecordResource : Resource
    {
        public string State { get; set; }
        public Hyperlink<BookResource> Book { get; set; }

        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public TimeSpan Span { get; set; }

        public string UserId { get; set; }
    }
}
