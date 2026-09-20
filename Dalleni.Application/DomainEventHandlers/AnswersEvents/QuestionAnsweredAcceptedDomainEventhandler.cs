using Dalleni.Application.Common;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.QuestionsAnswersEvents
{
    public class QuestionAnsweredAcceptedDomainEventhandler
        : INotificationHandler<
            DomainEventNotification<QuestionAnsweredAcceptedDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<QuestionAnsweredAcceptedDomainEventhandler> _logger;

        public QuestionAnsweredAcceptedDomainEventhandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<QuestionAnsweredAcceptedDomainEventhandler> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            DomainEventNotification<QuestionAnsweredAcceptedDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug(
                "Question answer accepted: {QuestionId}",
                domainEvent.QuestionId);

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

            user.OnAnswerAccepted();

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

            // Do not notify the answer author
            // if they accepted their own answer.
            if (answer.UserId == domainEvent.UserId)
            {
                return;
            }

            await _notificationService.CreateAsync(
                recipientId: answer.UserId,
                actorId: domainEvent.UserId,
                type: NotificationType.AnswerAccepted,
                entityType: NotificationEntityType.Answer,
                entityId: answer.Id,
                channels:
                    NotificationChannel.InApp |
                    NotificationChannel.RealTime |
                    NotificationChannel.Push,
                title:"",
                message:"",
                cancellationToken);

            _logger.LogDebug(
                "Answer accepted notification created. AnswerId: {AnswerId}, RecipientId: {RecipientId}",
                answer.Id,
                answer.UserId);
        }
    }
}