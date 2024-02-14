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

    public override Task SendMessage()
    {
        throw new NotImplementedException();
    }

    public override Task<string> FindRegistrationByHandle(string handle)
    {
        throw new NotImplementedException();
    }

    public override async Task RegisterDevice(DeviceRegistration device)
    {
        RegistrationDescription registration = device.Platform switch
        {
            PlatformType.Fcm => new FcmRegistrationDescription(device.Handle),
            PlatformType.Apns => new AppleRegistrationDescription(device.Handle),
            _ => throw new ArgumentOutOfRangeException()
        };

        await SaveDevice(registration.PnsHandle);
    }

    private async Task SaveDevice(string pns)
    {
        throw new NotImplementedException();
    }
}