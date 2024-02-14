namespace PushNotificationExample;

public class ExpoNotificationHandler : NotificationHandler
{
    public override Task SendMessage()
    {
        throw new NotImplementedException();
    }

    public override Task RegisterDevice(DeviceRegistration device)
    {
        throw new NotImplementedException();
    }

    public override Task<string> FindRegistrationByHandle(string handle)
    {
        throw new NotImplementedException();
    }
}