using Microsoft.Data.SqlClient;
using System.Data;

namespace PushNotificationExample.Database;

public class NotificationCommand
{
    private SqlConnection Connection { get; set; }
    
    private string ConnectionString { get; set; }

    public NotificationCommand(string connectionString)
    {
        Connection = new SqlConnection(this.ConnectionString);
        ConnectionString = connectionString;
    }
    
    public async Task SaveDevice(string pns)
    {
        await using var command = new SqlCommand();
        
        command.Connection = this.Connection;
        command.CommandType = CommandType.Text;
        command.CommandText = "INSERT INTO table VALUES ('1', '2', '3')";

        command.ExecuteNonQuery();
    }
}