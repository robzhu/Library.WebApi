using System;
using DDD;

namespace Library.DomainModel
{
    public enum Condition
    {
        New,
        Good,

        /// <summary>
        /// a bit worn, but still good for circulation
        /// </summary>
        Used,

        /// <summary>
        /// Needs to be replaced. 
        /// </summary>
        Damaged,
    }

    public class Book : Entity
    {
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string ISBN { get; set; }
        public DateTime Published { get; set; }
        public Condition Condition {get;set; }
        public bool IsAvailable { get; set; }
        public string LendingRecordId { get; set; }

        public void SetAuthor( Author author )
        {
            if( author == null ) throw new ArgumentNullException( "author cannot be null" );
            AuthorId = author.Id;
        }

        internal void Checkout( LendingRecord lendingRecord )
        {
            if( !IsAvailable )
            {
                throw new DomainOperationException( "this book is not available for checkout" );
            }
            LendingRecordId = lendingRecord.Id;
            IsAvailable = false;
        }

        internal void Checkin()
        {
            if( IsAvailable )
            {
                throw new DomainOperationException( "this book is already checked in" );
            }
            IsAvailable = true;
            LendingRecordId = null;
        }
    }
}
