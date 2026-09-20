using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.AnswersVotesEvents
{
    public class VoteAnswerDomainEventHandler
        : INotificationHandler<
            DomainEventNotification<VoteAnswerDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<VoteAnswerDomainEventHandler> _logger;

        public VoteAnswerDomainEventHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<VoteAnswerDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<VoteAnswerDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Answer vote created. VoteId: {VoteId}, AnswerId: {AnswerId}, UserId: {UserId}, VoteType: {VoteType}",
                domainEvent.VoteId,
                domainEvent.AnswerId,
                domainEvent.UserId,
                domainEvent.Type);

            var user = await _unitOfWork.Users.GetByIdAsync(
                domainEvent.UserId,
                true,
                cancellationToken);

            var vote = await _unitOfWork.Votes.GetByIdAsync(
                domainEvent.VoteId,
                true,
                cancellationToken);

            var answer = await _unitOfWork.Answers.GetByIdAsync(
                domainEvent.AnswerId,
                true,
                cancellationToken);

            if (user is null)
            {
                _logger.LogWarning(
                    "User {UserId} was not found for vote {VoteId}.",
                    domainEvent.UserId,
                    domainEvent.VoteId);

                return;
            }

            if (vote is null)
            {
                _logger.LogWarning(
                    "Vote {VoteId} was not found.",
                    domainEvent.VoteId);

                return;
            }

            if (answer is null)
            {
                _logger.LogWarning(
                    "Answer {AnswerId} was not found for vote {VoteId}.",
                    domainEvent.AnswerId,
                    domainEvent.VoteId);

                return;
            }

            // Existing reputation/business logic
            if (domainEvent.Type == VoteType.Upvote)
            {
                user.OnReceiveUpVote();
            }
            else if (domainEvent.Type == VoteType.Downvote)
            {
                user.OnReceiveDownVote();
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(domainEvent.Type),
                    domainEvent.Type,
                    "Unsupported answer vote type.");
            }

            // Do not notify the answer author
            // if they somehow voted on their own answer.
            if (answer.UserId == domainEvent.UserId)
            {
                return;
            }

            var notificationType = domainEvent.Type switch
            {
                VoteType.Upvote => NotificationType.AnswerUpvoted,
                VoteType.Downvote => NotificationType.AnswerDownvoted,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(domainEvent.Type),
                    domainEvent.Type,
                    "Unsupported answer vote type.")
            };

            await _notificationService.CreateAsync(
                recipientId: answer.UserId,
                actorId: domainEvent.UserId,
                type: notificationType,
                entityType: NotificationEntityType.Answer,
                entityId: answer.Id,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                    title:"Someone Vote on Your Answer",
                    message:"Your Answer Has beed Voted",
                cancellationToken);

            _logger.LogDebug(
                "Answer vote notification created. AnswerId: {AnswerId}, RecipientId: {RecipientId}, Type: {NotificationType}",
                answer.Id,
                answer.UserId,
                notificationType);
        }
    }
}