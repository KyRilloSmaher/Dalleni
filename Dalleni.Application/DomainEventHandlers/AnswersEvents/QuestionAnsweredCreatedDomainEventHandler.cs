using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DomainEventHandlers.AnswersEvents
{
    class QuestionAnsweredCreatedDomainEventHandler : INotificationHandler<DomainEventNotification<QuestionAnsweredDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<QuestionAnsweredCreatedDomainEventHandler> _logger;

        public QuestionAnsweredCreatedDomainEventHandler(
            IUnitOfWork unitOfWork,
            ILogger<QuestionAnsweredCreatedDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<QuestionAnsweredDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug($"Answer Deleted: {domainEvent.AnswerId}");

            var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);
            var question = await _unitOfWork.Questions.GetByIdAsync(domainEvent.QuestionId, true);

            if (user != null && question != null)
            {
                user.OnAnswerCreated();
                question.OnAnswerAdded();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }

            _logger.LogWarning(user is null ? "No User Found !" : "Question Not Found !");
        }
    }
}
