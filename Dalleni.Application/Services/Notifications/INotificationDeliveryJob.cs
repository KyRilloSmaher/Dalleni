using System;
using System.Threading.Tasks;

namespace Dalleni.Application.Services.Notifications
{
    public interface INotificationDeliveryJob
    {
        Task ExecuteAsync(Guid notificationId);
    }
}