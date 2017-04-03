using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;

class EmitLogTopic
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" ,Port = 5682};
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";

            channel.QueueDeclare(queue: routingKey,
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

            channel.ExchangeDeclare(exchange: "topic_logs",
                                    type: "topic");

          
            var message = (args.Length > 1)
                          ? string.Join(" ", args.Skip(1).ToArray())
                          : "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "topic_logs",
                                 routingKey: routingKey,
                                 basicProperties: properties,
                                 body: body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
        }
    }
}