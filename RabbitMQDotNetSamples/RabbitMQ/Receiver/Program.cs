using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5673 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                foreach (var severity in args)
                {
                    channel.QueueDeclare(queue: severity,
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                    var consumer2 = new EventingBasicConsumer(channel);
                    consumer2.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received {0} {1}", message, routingKey);
                    };
                    channel.BasicConsume(queue: severity,
                                         noAck: true,
                                         consumer: consumer2);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
