using System.Collections.Generic;
using HttpEx.REST;

namespace Library.DataTransferObjects
{
    public class AuthorResource : Resource
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Hyperlink<BookResource>> Books { get; set; }
    }
}
