
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Members.AcceptInvitation
{
    internal sealed class AcceptEntityInvitationCommandHandler: IRequestHandler< AcceptEntityInvitationCommand,Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvitationTokenGeneratorService _invitationTokenGeneratorService;

        public AcceptEntityInvitationCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            IInvitationTokenGeneratorService invitationTokenGeneratorService)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _invitationTokenGeneratorService = invitationTokenGeneratorService;
        }

        public async Task<Response<bool>> Handle(AcceptEntityInvitationCommand request,CancellationToken cancellationToken)
        {
          

            if (request.currentUserId == Guid.Empty)
            {
                return _responseHandler.Unauthorized<bool>(
                    "You must be authenticated.");
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return _responseHandler.BadRequest<bool>(
                    "Invitation token is required.");
            }

            var tokenHash =_invitationTokenGeneratorService.HashToken(request.Token);

            var invitation =await _unitOfWork.OfficialEntityInvitations.GetByTokenHashAsync(tokenHash,cancellationToken);

            if (invitation is null)
            {
                return _responseHandler.BadRequest<bool>("Invalid invitation.");
            }

            if (invitation.IsAccepted)
            {
                return _responseHandler.BadRequest<bool>("Invitation has already been accepted.");
            }

            if (invitation.IsExpired)
            {
                return _responseHandler.BadRequest<bool>("Invitation has expired.");
            }

            var user =await _unitOfWork.UserManager.FindByIdAsync(request.currentUserId.ToString());

            if (user is null)
            {
                return _responseHandler.NotFound<bool>("User account was not found.");
            }

            if (string.IsNullOrWhiteSpace(user.Email) ||
                !string.Equals(
                    user.Email.Trim(),
                    invitation.Email,
                    StringComparison.OrdinalIgnoreCase))
            {
                return _responseHandler.Unauthorized<bool>("This invitation was sent to a different email address.");
            }

            var existingMembership =
                await _unitOfWork.OfficialEntityMemberships
                    .GetByUserAndEntityAsync(
                        request.currentUserId,
                        invitation.OfficialEntityId,
                        cancellationToken);

            if (existingMembership is not null)
            {
                return _responseHandler.BadRequest<bool>(
                    "You are already a member of this entity.");
            }

            var entity =await _unitOfWork.OfficialEntities.GetByIdAsync(invitation.OfficialEntityId);

            if (entity is null)
            {
                return _responseHandler.NotFound<bool>(
                    "Official entity was not found.");
            }

            await _unitOfWork.BeginTransactionAsync(
                cancellationToken);

            try
            {
                var membership =OfficialEntityMembership.Create(invitation.OfficialEntityId,request.currentUserId,invitation.Role);

                await _unitOfWork.OfficialEntityMemberships.AddAsync(membership,cancellationToken);

                invitation.Accept();

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return _responseHandler.Success(true,$"You have joined {entity.Name} successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}