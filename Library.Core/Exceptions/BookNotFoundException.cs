using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(int id)
            : base($"Book with ID {id} was not found.") { }
    }
}
