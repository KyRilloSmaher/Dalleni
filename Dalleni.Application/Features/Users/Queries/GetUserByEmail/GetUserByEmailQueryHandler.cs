using AutoMapper;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Response<UserResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByEmailQueryHandler> _logger;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper, ILogger<GetUserByEmailQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {

            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning("User with email {Email} not found", request.Email);
                return _responseHandler.BadRequest<UserResponseDto>(SystemMessages.NOT_FOUND);
            }
            var userDto = _mapper.Map<UserResponseDto>(user);
            return _responseHandler.Success(userDto, SystemMessages.SUCCESS);
        }
    }
}
