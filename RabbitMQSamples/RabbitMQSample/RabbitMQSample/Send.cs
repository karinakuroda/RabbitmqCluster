using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.IO;

namespace RabbitMQSample
{
    class Send
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5682 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rabbitteste",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
                for (int i = 0; i <1; i++)
                {
                    var file = File.ReadAllBytes(@"..\..\HTMLPage1.html");

                    var body = file;//Encoding.UTF8.GetBytes(file);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                               routingKey: "rabbitteste",
                               basicProperties: properties,
                               body: body);
                   
                    Console.WriteLine(" [x] Sent {0}", i);
                }
               
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
