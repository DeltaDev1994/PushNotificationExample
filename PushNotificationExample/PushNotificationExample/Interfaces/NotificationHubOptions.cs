using System.ComponentModel.DataAnnotations;

public class NotificationHubOptions
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}