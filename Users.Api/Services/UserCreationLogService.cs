using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Users.Api.Models;

namespace Users.Api.Services
{
    /// <summary>
    /// Interface for logging user creation
    /// </summary>
    public interface IUserCreationLogService
    {
        /// <summary>
        /// Logs the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Log(User user);
    }

    /// <summary>
    /// Logs user creating using the factory client of RabbitMQ
    /// </summary>    
    /// <remarks>
    /// Sends user ID to be logged.
    /// </remarks>
    /// <params>
    /// 
    /// </params>
    public class UserCreationLogService : IUserCreationLogService
    {
        private const string QueueName = "usercreationlog";
        const string AmqpRabbit = "rabbitmq";
        const int RetryCount = 10;

        /// <summary>
        /// Logs the creation of user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Log(User user)
        {

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    Console.WriteLine(ex.ToString());
                });

            policy.Execute(() =>
            {

                var factory = new ConnectionFactory()
                {
                    //Uri = new Uri(AmqpRabbit),
                    HostName = AmqpRabbit,
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                    RequestedHeartbeat = 60,
                    Ssl =
                    {
                        ServerName = AmqpRabbit,
                        Enabled = false
                    }
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: QueueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        string message = String.Format("user with id:{0} was created.", user.Id);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                            routingKey: QueueName,
                            basicProperties: null,
                            body: body);

                    }
                }
            });
        }
    }
}
