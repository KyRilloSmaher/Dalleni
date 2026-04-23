using Dalleni.Application.DTOs.Requests.Base;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Validators.Base
{
    public class PagedRequestValidators : AbstractValidator<PagedRequest>
    {
        public PagedRequestValidators() {
            RuleFor(r => r.PageNumber)
                   .GreaterThan(0)
                   .WithMessage("Page Number Should be Positive integer");
            RuleFor(r => r.PageSize)
                .GreaterThan(0)
                .WithMessage("Page Size Should be Positive integer");
        }
    }
}
