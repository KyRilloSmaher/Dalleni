using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.QuestionsAnswersEvents
{
    public class MarkAnswerSuccessedDomainEventHandler
        : INotificationHandler<
            DomainEventNotification<MarkAnswerSuccessedDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MarkAnswerSuccessedDomainEventHandler> _logger;

        public MarkAnswerSuccessedDomainEventHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<MarkAnswerSuccessedDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<MarkAnswerSuccessedDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Mark answer succeeded: {AnswerId}",
                domainEvent.AnswerId);

            var user = await _unitOfWork.Users.GetByIdAsync(
                domainEvent.UserId,
                true,
                cancellationToken);

            if (user is null)
            {
                _logger.LogWarning(
                    "User {UserId} was not found.",
                    domainEvent.UserId);

                return;
            }

            user.OnAnswerSuccessed();

            var answer = await _unitOfWork.Answers.GetByIdAsync(
                domainEvent.AnswerId,
                true,
                cancellationToken);

            if (answer is null)
            {
                _logger.LogWarning(
                    "Answer {AnswerId} was not found.",
                    domainEvent.AnswerId);

                return;
            }

            // Do not notify the user who performed the action
            // if they are also the answer author.
            if (answer.UserId == domainEvent.UserId)
            {
                return;
            }

            await _notificationService.CreateAsync(
                recipientId: answer.UserId,
                actorId: domainEvent.UserId,
                type: NotificationType.AnswerSuccessful,
                entityType: NotificationEntityType.Answer,
                entityId: answer.Id,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                title : "Your Question Marked Suceesed",
                message: "Your Question Has been Marked Suceesed",
                cancellationToken);

            _logger.LogDebug(
                "Answer success notification created. AnswerId: {AnswerId}, RecipientId: {RecipientId}",
                answer.Id,
                answer.UserId);
        }
    }
}