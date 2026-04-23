using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Exceptions
{
    public class ValidationException:AppException
    {
        public List<string> Errors { get; set; }

        public ValidationException(List<string> errors)
            : base("Validation failed", 400)
        {
            Errors = errors;
        }
    }
}
