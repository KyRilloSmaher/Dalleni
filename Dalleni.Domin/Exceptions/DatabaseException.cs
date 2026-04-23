using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Exceptions
{
    public class DatabaseException : AppException
    {
        public DatabaseException(string message = "Database operation failed")
            : base(message, 500)
        {
        }
    }
}
