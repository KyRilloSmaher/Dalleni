using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Application.DTOs.Responses;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Requests.Auth;
namespace Dalleni.Application.Features.Authantications.ConfirmEmail
{
    public record ConfirmEmailCommand(ConfirmEmailRequest dto) : IRequest<Response<bool>>;
}

