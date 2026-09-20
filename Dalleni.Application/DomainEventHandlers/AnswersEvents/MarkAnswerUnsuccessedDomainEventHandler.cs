using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.QuestionsAnswersEvents
{
    public class MarkAnswerUnSuccessedDomainEventHandler
        : INotificationHandler<
            DomainEventNotification<MarkAnswerUnsuccessedDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MarkAnswerUnSuccessedDomainEventHandler> _logger;

        public MarkAnswerUnSuccessedDomainEventHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<MarkAnswerUnSuccessedDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<MarkAnswerUnsuccessedDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Mark answer un succeeded: {AnswerId}",
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

            user.OnAnswerFailed();

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
                type: NotificationType.AnswerUnsuccessful,
                entityType: NotificationEntityType.Answer,
                entityId: answer.Id,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                title : "Your Question Marked unSuceesed",
                message: "Your Question Has been Marked unSuceesed",
                cancellationToken);

            _logger.LogDebug(
                "Answer unsuccess notification created. AnswerId: {AnswerId}, RecipientId: {RecipientId}",
                answer.Id,
                answer.UserId);
        }
    }
}