using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    class RabbitMQ
    {
        static void Main(string[] args)
        {

            var argsReceiver = new string[3];
            argsReceiver[0] = "error";
            argsReceiver[1] = "info";
            argsReceiver[2] = "warning";

            //1. SEND MESSAGE FOR EXCHANGE - CREATE QUEUE
            SendExchange.Program.Main(argsReceiver);

            //2. READ FROM QUEUE
            //Receiver.Program.Main(argsReceiver);
        }
    }
}
