using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Responses;
using Dalleni.Application.DTOs.Requests.Auth;
namespace Dalleni.Application.Features.Authantications.SignUp
{
    public record SignUpCommand(SignUpRequest dto) : IRequest<Response<bool>>;
}
