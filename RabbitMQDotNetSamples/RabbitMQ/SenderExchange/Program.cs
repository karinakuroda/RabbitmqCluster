using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenderExchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5673 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                var message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                foreach (var severity in args)
                {
                    channel.BasicPublish(exchange: "direct_logs",
                                       routingKey: severity,
                                       basicProperties: null,
                                       body: body);
                }
                Console.WriteLine(" [x] Sent {0}", message);
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
                   ? string.Join(" ", args)
                   : "info: Hello World!");
        }
    }
}
