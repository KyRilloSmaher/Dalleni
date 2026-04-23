using AutoMapper;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Application.Features.Users.Commands.RestoreAccount;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Domin.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.UpdateRrofileImage
{
    public class UpdateProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly ILogger<UpdateProfileImageCommandHandler> _logger;

        public UpdateProfileImageCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IImageUploaderService imageUploaderService, ILogger<UpdateProfileImageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _imageUploaderService = imageUploaderService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(UpdateProfileImageCommand command, CancellationToken cancellationToken)
        {
            var userId = command.request.Id;
            var newImage = command.request.ProfileImage;

            // Fetch user via Identity
            var user =  await _unitOfWork.Users.GetByIdAsync(userId,true);
            if (user == null)
                return _responseHandler.NotFound<string>(SystemMessages.USER_NOT_FOUND);

            // Delete old image
            var oldImageUrl = user.ProfileImageUrl;
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                var deleteResult = await _imageUploaderService.DeleteImageByUrlAsync(oldImageUrl);
                if (!deleteResult)
                    return _responseHandler.UnprocessableEntity<string>(SystemMessages.FAILED);
            }


            // Upload new image
            var uploadResult = await _imageUploaderService.UploadImageAsync(newImage, ImageFolder.profiles);
            if (uploadResult.Error != null)
                return _responseHandler.BadRequest<string>(SystemMessages.FILE_UPLOAD_FAILED);

            // Update user profile image
            user.ProfileImageUrl = uploadResult.Url.ToString();

            // Save changes through UnitOfWork
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success(user.ProfileImageUrl);
        }
    }
}
