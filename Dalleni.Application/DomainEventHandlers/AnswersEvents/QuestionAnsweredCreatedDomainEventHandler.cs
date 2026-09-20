using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.AnswersEvents
{
    public class QuestionAnsweredCreatedDomainEventHandler
        : INotificationHandler<
            DomainEventNotification<QuestionAnsweredDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<QuestionAnsweredCreatedDomainEventHandler> _logger;

        public QuestionAnsweredCreatedDomainEventHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<QuestionAnsweredCreatedDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<QuestionAnsweredDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Answer created: {AnswerId}",
                domainEvent.AnswerId);

            var user = await _unitOfWork.Users.GetByIdAsync(
                domainEvent.UserId,
                true,
                cancellationToken);

            var question = await _unitOfWork.Questions.GetByIdAsync(
                domainEvent.QuestionId,
                true,
                cancellationToken);

            if (user is null)
            {
                _logger.LogWarning(
                    "User {UserId} was not found.",
                    domainEvent.UserId);

                return;
            }

            if (question is null)
            {
                _logger.LogWarning(
                    "Question {QuestionId} was not found.",
                    domainEvent.QuestionId);

                return;
            }

            // Existing business logic
            user.OnAnswerCreated();
            question.OnAnswerAdded();

            // Do not notify the question owner
            // if they answered their own question.
            if (question.UserId == domainEvent.UserId)
            {
                return;
            }

            await _notificationService.CreateAsync(
                recipientId: question.UserId,
                actorId: domainEvent.UserId,
                type: NotificationType.NewAnswer,
                entityType: NotificationEntityType.Answer,
                entityId: domainEvent.AnswerId,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                title:"New Answer Added",
                message:"New Answer was Added To your Question",
                cancellationToken);

            _logger.LogDebug(
                "New answer notification created. AnswerId: {AnswerId}, RecipientId: {RecipientId}",
                domainEvent.AnswerId,
                question.UserId);
        }
    }
}