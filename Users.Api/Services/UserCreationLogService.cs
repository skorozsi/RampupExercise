using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Users.Api.Models;

namespace Users.Api.Services
{
    public interface IUserCreationLogService
    {
        void Log(User user);
    }

    public class UserCreationLogService : IUserCreationLogService
    {
        public void Log(User user)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "usercreationlog",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = String.Format("user with id:{0} was created.", user.Id);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "usercreation",
                        basicProperties: null,
                        body: body);
                }
            }

        }
    }
}
