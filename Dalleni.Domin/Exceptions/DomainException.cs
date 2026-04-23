using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Exceptions
{
    public class DomainException:AppException
    {

        public DomainException(string message) :base(message, 422)
        {
        }
    }
}
