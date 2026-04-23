using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class QuestionDeletedDomainEventHandle : INotificationHandler<DomainEventNotification<QuestionDeletedDomainEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<QuestionDeletedDomainEventHandle> _logger;

    public QuestionDeletedDomainEventHandle(
        IUnitOfWork unitOfWork,
        ILogger<QuestionDeletedDomainEventHandle> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle( DomainEventNotification<QuestionDeletedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogDebug($"Question created: {domainEvent.QuestionId}");

        var user = await _unitOfWork.Users.GetByIdAsync(domainEvent.UserId, true);

        if (user != null)
        {
            user.OnQuestionDeleted();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        _logger.LogWarning("No User Found !");
    }
}