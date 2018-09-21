using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(20000);
            Console.WriteLine(" Consumer was started.");
            var amqpRabbit = "rabbitmq";
            var factory = new ConnectionFactory()
            {
                //Uri = new Uri(amqpRabbit),
                HostName = amqpRabbit,
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                RequestedHeartbeat = 60,
                Ssl =
                {
                    ServerName = amqpRabbit,
                    Enabled = false
                }
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "usercreationlog",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: "usercreationlog",
                        autoAck: true,
                        consumer: consumer);

                    //Console.WriteLine(" Press [enter] to exit.");
                    while (1 == 1){}
                    //Console.ReadLine();
                }
            }
        }

    }
}
