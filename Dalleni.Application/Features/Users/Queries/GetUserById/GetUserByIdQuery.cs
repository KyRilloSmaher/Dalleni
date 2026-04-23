using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdlQuery
     (
    Guid Id
   ) : IRequest<Response<UserResponseDto>>;
}
