using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Queries.SearchUserByName
{
    public record SearchUsersByNameQuery
      (
     string keyword
    ) : IRequest<Response<List<UserResponseDto>>>;
}
