using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.ExternalServicesAbstractions
{
    public interface IImageUploaderServiceFactory
    {
        IImageUploaderService Create();
    }
}
