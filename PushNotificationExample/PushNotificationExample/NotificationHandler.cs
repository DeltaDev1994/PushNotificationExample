using System.Reflection.Metadata;
using Microsoft.Azure.NotificationHubs;

namespace PushNotificationExample;
using System.Net.Http;
using System.Threading;
using System.Security.Principal;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PlatformType {
    Mpns = 1,
    Wns = 2,
    Apns = 3,
    Fcm = 4,
}

public abstract class NotificationHandler
{
    public abstract Task<NotificationOutcome> SendMessage(NotificationPlatform platform, string id, string message);

    public abstract Task RegisterDevice(DeviceRegistration device);

    public abstract Task<string> FindRegistrationByHandle(string handle);
}