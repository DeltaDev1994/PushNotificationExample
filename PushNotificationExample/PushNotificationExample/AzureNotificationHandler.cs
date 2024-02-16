using Microsoft.Azure.NotificationHubs;

namespace PushNotificationExample;

public class AzureNotificationHandler : NotificationHandler
{
    public NotificationHubClient Hub { get; set; }
    private string ConnectionString { get; set; }
    private string NotificationHub { get; set; }

    public AzureNotificationHandler(string connectionString, string notificationHub)
    {
        this.ConnectionString = connectionString;
        this.NotificationHub = notificationHub;
        Hub = NotificationHubClient.CreateClientFromConnectionString(this.ConnectionString, this.NotificationHub);
    }

    public override async Task<NotificationOutcome> SendMessage(NotificationPlatform platform, string id, string message)
    {
        switch (platform)
        {
            case NotificationPlatform.Apns:
            {
                var installation = new Installation
                {
                    InstallationId = "fake-apns-install-id",
                    Platform = platform,
                    PushChannel = id
                };
                    
                await Hub.CreateOrUpdateInstallationAsync(installation);
                var outcome = await Hub.SendAppleNativeNotificationAsync(message);
                
                return outcome;
            }

            case NotificationPlatform.Fcm:
            {
                var installation = new Installation
                {
                    InstallationId = "fake-fcm-install-id",
                    Platform = platform,
                    PushChannel = id
                };
                
                await Hub.CreateOrUpdateInstallationAsync(installation);
                var outcome = await Hub.SendFcmNativeNotificationAsync(message);
                
                return outcome;
            }
            
            default:
                return null;
        }
    }

    public override Task<string> FindRegistrationByHandle(string handle)
    {
        throw new NotImplementedException();
    }
    
    
    // Registrations is an older way of registering devices with AZN.
    // Installations is the latest and best way of registering devices with AZN.
    public override async Task RegisterDevice(DeviceRegistration device)
    {
        switch (device.Platform)
        {
            case PlatformType.Apns:
                var installation = new Installation
                {
                    InstallationId = "fake-apns-install-id",
                    Platform = NotificationPlatform.Apns,
                    PushChannel = device.Handle
                };
                    
                await Hub.CreateOrUpdateInstallationAsync(installation);
                break;
        }

        // await SaveDevice(registration.PnsHandle);
    }

    private async Task SaveDevice(string pns)
    {
        throw new NotImplementedException();
    }
}