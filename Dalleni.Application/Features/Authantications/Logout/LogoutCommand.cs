using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
namespace Dalleni.Application.Features.Authantications.Logout
{
    public record LogoutCommand(string userId) : IRequest<Response<bool>>;
}
