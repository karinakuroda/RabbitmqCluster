using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiverExchange
{
    public class Program
    {
       public  static void Main(string[] args)
        {
            //CONFIGURANDO EXCHANGE
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5673 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                foreach (var severity in args)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_logs",
                                      routingKey: severity);

                    var consumer2 = new EventingBasicConsumer(channel);
                    consumer2.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received from exchange {0} {1}", message, routingKey);

                        //CONFIGURA QUEUES
                        channel.QueueDeclare(queue: severity,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        channel.BasicPublish(exchange: "",
                                   routingKey: severity,
                                   basicProperties: properties,
                                   body: ea.Body);
                    };
                    channel.BasicConsume(queue: queueName,
                               noAck: true,
                               consumer: consumer2);

                }

             
                //ENVIA MSG
                using (var connection2 = factory.CreateConnection())
                using (var channel2 = connection2.CreateModel())
                {
                    channel2.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                    var message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    foreach (var severity in args)
                    {
                        channel2.BasicPublish(exchange: "direct_logs",
                                           routingKey: severity,
                                           basicProperties: null,
                                           body: body);
                    }
                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
