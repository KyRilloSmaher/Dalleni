using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.AnswersVotesEvents
{
    public class VoteQuestionDomainEventHandler
        : INotificationHandler<
            DomainEventNotification<VoteQuestionDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<VoteQuestionDomainEventHandler> _logger;

        public VoteQuestionDomainEventHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<VoteQuestionDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<VoteQuestionDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Vote Question created: {QuestionId}",
                domainEvent.QuestionId);

            var user = await _unitOfWork.Users.GetByIdAsync(
                domainEvent.UserId,
                true,
                cancellationToken);

            var vote = await _unitOfWork.Votes.GetByIdAsync(
                domainEvent.VoteId,
                true,
                cancellationToken);

            var question = await _unitOfWork.Questions.GetByIdAsync(
                domainEvent.QuestionId,
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

            if (question is null)
            {
                _logger.LogWarning(
                    "Question {QuestionId} was not found.",
                    domainEvent.QuestionId);

                return;
            }

            // Do not notify the question owner about their own vote.
            if (question.UserId == domainEvent.UserId)
            {
                return;
            }

            var notificationType =
                vote.Type == VoteType.Upvote
                    ? NotificationType.QuestionUpvoted
                    : NotificationType.QuestionDownvoted;

            await _notificationService.CreateAsync(
                recipientId: question.UserId,
                actorId: domainEvent.UserId,
                type: notificationType,
                entityType: NotificationEntityType.Question,
                entityId: question.Id,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                title : "Your Question  Voted",
                message: "Your Question Has been Voted",
                cancellationToken);
        }
    }
}