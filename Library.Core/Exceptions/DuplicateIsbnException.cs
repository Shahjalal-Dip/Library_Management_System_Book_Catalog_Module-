using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Exceptions
{
    public class DuplicateIsbnException : Exception
    {
        public DuplicateIsbnException(string isbn)
            : base($"A book with ISBN '{isbn}' already exists.") { }
    }
}
