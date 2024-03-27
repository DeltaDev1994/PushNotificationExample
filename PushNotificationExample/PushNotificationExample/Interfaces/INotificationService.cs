using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PushNotificationExample.Models;


namespace PushNotificationExample.Services
{
    public interface INotificationService
    {
        Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token);
        Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token);
        Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token);
    }
}