using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class QuestionCreatedDomainEventHandler: INotificationHandler<DomainEventNotification<QuestionCreatedDomainEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<QuestionCreatedDomainEventHandler> _logger;

    public QuestionCreatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        ILogger<QuestionCreatedDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle( DomainEventNotification<QuestionCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogDebug($"Question created: {domainEvent.QuestionId}");

        var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);

        if (user != null)
        {
            user.OnQuestionCreated();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        _logger.LogWarning("No User Found !");
    }
}