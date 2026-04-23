using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Requests.Base
{
    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
