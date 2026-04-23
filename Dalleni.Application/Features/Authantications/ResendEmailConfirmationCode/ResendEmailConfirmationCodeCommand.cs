using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Responses;
using Dalleni.Application.DTOs.Requests.Auth;
namespace Dalleni.Application.Features.Authantications.ResendEmailConfirmationCode
{
    public record ResendConfirmaionEmailCommand(ReSendConfirmationEmailRequest dto) : IRequest<Response<bool>>;
}
