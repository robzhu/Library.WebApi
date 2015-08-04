using System;

namespace Library.DomainModel
{
    [Serializable]
    public class DomainOperationException : Exception
    {
        public DomainOperationException() : base() { }
        public DomainOperationException( string message ) : base( message ) { }
    }
}
