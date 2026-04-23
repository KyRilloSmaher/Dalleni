using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DomainEventHandlers.AnswersVotesEvents
{
    class VoteAnswerDomainEventHandler : INotificationHandler<DomainEventNotification<VoteAnswerDomainEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoteAnswerDomainEventHandler> _logger;

        public VoteAnswerDomainEventHandler(
            IUnitOfWork unitOfWork,
            ILogger<VoteAnswerDomainEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<VoteAnswerDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogDebug($"Vote Question created: {domainEvent.VoteId}");

            var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);
            var vote = await _unitOfWork.Votes.GetByIdAsync(domainEvent.VoteId, true);
            var answer = await _unitOfWork.Answers.GetByIdAsync(domainEvent.AnswerId, true);
            if (user is not null && vote is not null)
            {

                if (domainEvent.Type == VoteType.Upvote)
                {
                    user.OnReceiveUpVote();
                }
                else
                {
                    user.OnReceiveDownVote();
                }
            }


            _logger.LogWarning(user is null ? "No User Found !" : "No Vote Found !");
        }
    }
}
