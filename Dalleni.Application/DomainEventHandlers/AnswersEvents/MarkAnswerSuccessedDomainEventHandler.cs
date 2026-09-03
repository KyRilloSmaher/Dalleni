
using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class MarkAnswerSuccessedDomainEventHandler: INotificationHandler<DomainEventNotification<MarkAnswerSuccessedDomainEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAnswerSuccessedDomainEvent> _logger;

    public MarkAnswerSuccessedDomainEventHandler(
        IUnitOfWork unitOfWork,
        ILogger<MarkAnswerSuccessedDomainEvent> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle( DomainEventNotification<MarkAnswerSuccessedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogDebug($"Mark answer succeeded: {domainEvent.AnswerId}");

        var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);

        if (user != null)
        {
            user.OnAnswerSuccessed();
            return;
        }

        _logger.LogWarning("No User Found !");
    }
}