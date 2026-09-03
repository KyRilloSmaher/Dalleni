
using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class MarkAnswerUnsuccessedDomainEventHandler: INotificationHandler<DomainEventNotification<MarkAnswerUnsuccessedDomainEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAnswerUnsuccessedDomainEvent> _logger;

    public MarkAnswerUnsuccessedDomainEventHandler(
        IUnitOfWork unitOfWork,
        ILogger<MarkAnswerUnsuccessedDomainEvent> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle( DomainEventNotification<MarkAnswerUnsuccessedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogDebug($"Mark answer failed: {domainEvent.AnswerId}");

        var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);

        if (user != null)
        {
            user.OnAnswerFailed();
            return;
        }

        _logger.LogWarning("No User Found !");
    }
}