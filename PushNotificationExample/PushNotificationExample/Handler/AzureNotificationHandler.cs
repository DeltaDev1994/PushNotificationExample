using Microsoft.Azure.NotificationHubs;
using PushNotificationExample.Database;

namespace PushNotificationExample;

public class PushTemplates
{
    public class Generic
    {
        public const string Android = "{ \"notification\": { \"title\" : \"PushDemo\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"action\" : \"$(alertAction)\" } }";
        public const string iOS = "{ \"aps\" : {\"alert\" : \"$(alertMessage)\"}, \"action\" : \"$(alertAction)\" }";
    }

    public class Silent
    {
        public const string Android = "{ \"data\" : {\"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\"} }";
        public const string iOS = "{ \"aps\" : {\"content-available\" : 1, \"apns-priority\": 5, \"sound\" : \"\", \"badge\" : 0}, \"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\" }";
    }
}

public class AzureNotificationHandler : NotificationHandler
{
    public NotificationHubClient Hub { get; set; }
    
    private string ConnectionString { get; set; }
    
    private string NotificationHub { get; set; }
    
    private string PrepareNotificationPayload(string template, string text, string action) => template
        .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
        .Replace("$(alertAction)", action, StringComparison.InvariantCulture);
    
    
    public AzureNotificationHandler()
    {
        ConnectionString = "Endpoint=sb://MyNavy-Notification-Hub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aB4p1yHHzmU5xMVqaTRcBUVffAfOJGWxHirBws/HDv4=";
        NotificationHub = "MyNavy-prod-hub";
        Hub = NotificationHubClient.CreateClientFromConnectionString(this.ConnectionString, this.NotificationHub);
    }

    public override async Task SendMessage(PlatformType platform, string pns, string message)
    {
        string payload = null;
        switch (platform)
        {
            case PlatformType.Fcm:
                payload = PrepareNotificationPayload(PushTemplates.Generic.Android, message, "default");
                await Hub.SendFcmNativeNotificationAsync(payload);
                break;
            
            case PlatformType.Apns:
                payload = PrepareNotificationPayload(PushTemplates.Generic.iOS, message, "default");
                await Hub.SendAppleNativeNotificationAsync(payload);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
        }
    }

    public override async Task<string> FindRegistrationByHandle(string handle)
    {
        var reg = await Hub.GetRegistrationAsync<RegistrationDescription>(handle);
        return reg.PnsHandle;
    }

    public override async Task<RegistrationDescription> RegisterDevice(DeviceRegistration device)
    {
        RegistrationDescription registration = device.Platform switch
        {
            PlatformType.Fcm => new FcmRegistrationDescription(device.Handle),
            PlatformType.Apns => new AppleRegistrationDescription(device.Handle),
            _ => throw new ArgumentOutOfRangeException()
        };

        await Hub.CreateRegistrationAsync(registration);
        // await SaveDevice(registration.PnsHandle);
        
        return registration;
    }

    private async Task SaveDevice(string pns)
    {
        try
        {
            var command = new NotificationCommand("fake-connection-string");
            await command.SaveDevice(pns);
        }

        catch (Exception e)
        {
            Console.WriteLine($"Issue saving device in the correct location: {e.Message}");
        }
    }
}