using System;
using DDD;

namespace Library.DomainModel
{
    public enum LendingRecordState
    {
        OnLoan,
        Overdue,
        Returned,
    }

    /// <summary>
    /// Represents a duration in which a book is checked out to a user
    /// </summary>
    public class LendingRecord : Entity
    {
        private static TimeSpan DefaultLendingSpan = TimeSpan.FromDays( 14 );

        public static LendingRecord Create( string bookId, string userId )
        {
            return new LendingRecord
            {
                BookId = bookId,
                CheckoutDate = DateTime.Now,
                Span = DefaultLendingSpan,
                DueDate = DateTime.Now + DefaultLendingSpan,
                UserId = userId,
            };
        }

        public LendingRecordState State { get; set; }
        public string BookId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public TimeSpan Span { get; set; }
        public string UserId { get; set; }

        internal void Checkin()
        {
            ReturnedDate = DateTime.Now;
            State = LendingRecordState.Returned;
        }
    }
}
