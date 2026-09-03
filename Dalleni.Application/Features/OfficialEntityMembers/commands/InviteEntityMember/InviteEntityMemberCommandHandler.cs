using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.OfficialEntities.Members.Invite
{
    internal sealed class InviteEntityMemberCommandHandler
        : IRequestHandler<InviteEntityMemberCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IInvitationTokenGeneratorService _invitationTokenGenerator;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<InviteEntityMemberCommandHandler> _logger;

        public InviteEntityMemberCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            IInvitationTokenGeneratorService invitationTokenGenerator,
            IEmailService emailService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            ILogger<InviteEntityMemberCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _invitationTokenGenerator = invitationTokenGenerator;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(InviteEntityMemberCommand request,CancellationToken cancellationToken)
        {
            // 1. Get official entity
            var entity = await _unitOfWork.OfficialEntities.GetByIdAsync(request.OfficialEntityId);

            if (entity is null)
            {
                return _responseHandler.NotFound<bool>("Official entity was not found.");
            }

            // 2. Entity must be verified
            if (!entity.IsVerified)
            {
                return _responseHandler.BadRequest<bool>("Only verified official entities can manage members.");
            }

            // 3. Owner cannot be invited
            if (request.Role == EntityRole.Owner)
            {
                return _responseHandler.BadRequest<bool>("An owner cannot be assigned through an invitation.");
            }

            // 4. Check inviter membership and permissions
            var inviter =
                await _unitOfWork.OfficialEntityMemberships
                    .GetByUserAndEntityAsync(
                        request.currentuserId,
                        request.OfficialEntityId,
                        cancellationToken);

            if (inviter is null || !inviter.CanManageMembers())
            {
                return _responseHandler.Unauthorized<bool>( "You do not have permission to manage members.");
            }

            // 5. Normalize email
            var email = request.Email.Trim().ToLowerInvariant();

            // 6. Check if invited user already exists
            var invitedUser =await _unitOfWork.UserManager.FindByEmailAsync(email);

            if (invitedUser is not null)
            {
                var existingMembership =
                    await _unitOfWork.OfficialEntityMemberships
                        .GetByUserAndEntityAsync(
                            invitedUser.Id,
                            request.OfficialEntityId,
                            cancellationToken);

                if (existingMembership is not null)
                {
                    return _responseHandler.BadRequest<bool>(  "This user is already a member of the entity.");
                }
            }

            // 7. Check existing pending invitation
            var existingInvitation =
                await _unitOfWork.OfficialEntityInvitations
                    .GetPendingInvitationAsync(
                        request.OfficialEntityId,
                        email,
                        cancellationToken);

            if (existingInvitation is not null &&
                !existingInvitation.IsExpired)
            {
                return _responseHandler.BadRequest<bool>(
                    "A pending invitation already exists for this email.");
            }

            // 8. Generate invitation token
            var token = _invitationTokenGenerator.GenerateToken();

            var tokenHash =
                _invitationTokenGenerator.HashToken(token);

            // 9. Create invitation
            var invitation =
                OfficialEntityInvitation.Create(
                    request.OfficialEntityId,
                    request.currentuserId,
                    email,
                    request.Role,
                    tokenHash,
                    DateTime.UtcNow.AddDays(3));

            // 10. Save invitation
            await _unitOfWork.BeginTransactionAsync(
                cancellationToken);

            try
            {
                await _unitOfWork.OfficialEntityInvitations
                    .AddAsync(
                        invitation,
                        cancellationToken);

                await _unitOfWork.CommitTransactionAsync(
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                _logger.LogError(
                    ex,
                    "Failed to create invitation for {Email} " +
                    "to official entity {OfficialEntityId}.",
                    email,
                    request.OfficialEntityId);

                throw;
            }

            // 11. Generate BACKEND invitation URL
            var invitationUrl =
                _linkGenerator.GetUriByName(
                    _httpContextAccessor.HttpContext,
                    "AcceptOfficialEntityInvitation",
                    new
                    {
                        token
                    });

            if (string.IsNullOrWhiteSpace(invitationUrl))
            {
                _logger.LogError(
                    "Could not generate invitation URL for " +
                    "official entity {OfficialEntityId}.",
                    request.OfficialEntityId);

                return _responseHandler.BadRequest<bool>(
                    "Could not generate invitation link.");
            }

            // 12. Prepare email
            var emailBody = $"""
                You have been invited to join {entity.Name}
                as {request.Role}.

                Accept the invitation using the following link:

                {invitationUrl}

                This invitation expires in 3 days.
                """;

            // 13. Send invitation email
            var emailSent =
                await _emailService.SendEmailAsync(
                    email,
                    $"Invitation to join {entity.Name}",
                    emailBody);

            if (!emailSent)
            {
                _logger.LogWarning(
                    "Invitation was created but email could not be sent " +
                    "to {Email} for official entity {OfficialEntityId}.",
                    email,
                    request.OfficialEntityId);
            }

            // 14. Return success
            return _responseHandler.Success(
                true,
                "Invitation sent successfully.");
        }
    }
}