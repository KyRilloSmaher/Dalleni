using AutoMapper;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Application.Features.Users.Commands.UpdateRrofileImage;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dalleni.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Response<UserResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper, ILogger<UpdateProfileCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<UserResponseDto>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
        {
            var request = command.request;
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, true);
            if (user == null)
                return _responseHandler.NotFound<UserResponseDto?>(SystemMessages.USER_NOT_FOUND);

            var oldEmail = user.Email;

            // Update Identity fields if changed
            if (!string.IsNullOrEmpty(request.UserName) && request.UserName != user.UserName)
            {
                var identityUser = user;
                identityUser.UserName = request.UserName ?? identityUser.UserName;
                var identityUpdateResult = await _unitOfWork.UserManager.UpdateAsync(identityUser);
                if (!identityUpdateResult.Succeeded)
                    return _responseHandler.BadRequest<UserResponseDto?>(
                        string.Join(", ", identityUpdateResult.Errors.Select(e => e.Description))
                    );
            }

            // Map other user DTO fields
            _mapper.Map(request, user);
            // Save changes through UnitOfWork
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            // Map response DTO
            var updateduserDto = _mapper.Map<UserResponseDto?>(user);
            return _responseHandler.Success(updateduserDto);
        }
    }
}
