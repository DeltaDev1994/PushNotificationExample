using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;

namespace PushNotificationExample;

public class MessageDto
{
    public string Message { get; set; }
    
    public PlatformType Platform { get; set; }
    
    public string PNS { get; set; }
}

public class RegisterDto
{
    public string PNSHandle { get; set; }
    
    public PlatformType PlatformType { get; set; }
}

[Produces("application/json")]
[Route("api/v1/notifications")]
[ApiController]
public class NotificationController : Controller
{
    private AzureNotificationHandler Handler { get; }

    public NotificationController(AzureNotificationHandler handler)
    {
        Handler = handler;
    }
    
    // GET
    [HttpGet]
    public async Task<OkResult> GetNotifications()
    {
        return Ok();
    }

    [HttpPost("register")]
    public async Task<OkObjectResult> RegisterDevice([FromBody] RegisterDto message)
    {
        var reg = new DeviceRegistration
        {
            Platform = message.PlatformType,
            Handle = message.PNSHandle
        };
        
        var result = await Handler.RegisterDevice(reg);
        return Ok(result);
    }

    [HttpPost("send")]
    public async Task<OkResult> SendMessage([FromBody] MessageDto message)
    {
        await Handler.SendMessage(message.Platform, "fake-pns", message.Message);
        return Ok();
    }
}