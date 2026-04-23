

using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.DomainEventHandlers.AnswersVotesEvents
{
    class VoteQuestionDomainEventHandler : INotificationHandler<DomainEventNotification<VoteQuestionDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoteQuestionDomainEventHandler> _logger;

        public VoteQuestionDomainEventHandler(
            IUnitOfWork unitOfWork,
            ILogger<VoteQuestionDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<VoteQuestionDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug($"Vote Question created: {domainEvent.QuestionId}");

            var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);
            var vote = await _unitOfWork.Votes.GetByIdAsync(domainEvent.VoteId, true);
            var question = await _unitOfWork.Questions.GetByIdAsync(domainEvent.QuestionId, true);
            if (user is not null && vote is not null)
            {

                
            }


            _logger.LogWarning(user is null ? "No User Found !" : "No Vote Found !");
        }
    }
}
