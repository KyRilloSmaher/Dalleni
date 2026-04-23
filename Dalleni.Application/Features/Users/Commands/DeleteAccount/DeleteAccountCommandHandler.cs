using AutoMapper;
using Dalleni.Application.Features.Users.Queries.GetUserById;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteAccountCommandHandler> _logger;

        public DeleteAccountCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper, ILogger<DeleteAccountCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id,true);
            if (user is null)
            {
                _logger.LogInformation("User with ID {UserId} not found for deletion", request.Id);
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }
            user.Delete();
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                _logger.LogError("Failed to delete user with ID {UserId}", request.Id);
                return _responseHandler.ServerError<bool>(SystemMessages.SERVER_ERROR);
            }
            _logger.LogInformation("Successfully deleted user with ID {UserId}", request.Id);
            return _responseHandler.Success(true, SystemMessages.SUCCESS);

        }
    }
}
