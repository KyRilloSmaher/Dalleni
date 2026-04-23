using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery
      (
     string Email
    ) : IRequest<Response<UserResponseDto>>;
}
