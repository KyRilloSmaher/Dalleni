using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class QuestionAnsweredAcceptedDomainEventhandler: INotificationHandler<DomainEventNotification<QuestionAnsweredAcceptedDomainEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<QuestionAnsweredAcceptedDomainEvent> _logger;

    public QuestionAnsweredAcceptedDomainEventhandler(
        IUnitOfWork unitOfWork,
        ILogger<QuestionAnsweredAcceptedDomainEvent> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle( DomainEventNotification<QuestionAnsweredAcceptedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogDebug($"Question answered accepted: {domainEvent.QuestionId}");

        var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);

        if (user != null)
        {
            user.OnAnswerAccepted();
            return;
        }

        _logger.LogWarning("No User Found !");
    }
}