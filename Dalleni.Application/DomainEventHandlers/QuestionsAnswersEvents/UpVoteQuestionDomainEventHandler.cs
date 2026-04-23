using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DomainEventHandlers.QuestionsAnswersEvents
{
    class UpVoteQuestionDomainEventHandler : INotificationHandler<DomainEventNotification<VoteQuestionDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpVoteQuestionDomainEventHandler> _logger;

        public UpVoteQuestionDomainEventHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpVoteQuestionDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<VoteQuestionDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug($"UpVote Question created: {domainEvent.QuestionId}");

            var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);
            var question = await _unitOfWork.Questions.GetByIdAsync(domainEvent.QuestionId, true);

             

            _logger.LogWarning("No User Found !");
        }
    }
}
