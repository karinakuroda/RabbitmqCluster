﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQPublishSubscribe
{
    class EmitLog
    {
        public static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
               
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");
    
                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "logs",
                                         routingKey: "",
                                         basicProperties: properties,
                                         body: body);
        
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
