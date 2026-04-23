using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message): base(message, 400)
        {
        }
    }
}
