using System;
using RabbitMQ.Client;
using System.Text;

namespace Engine.MessageProducer
{
    public sealed class Sender
    {
        private static readonly Lazy<Sender>
            lazy =
            new Lazy<Sender>
                (() => new Sender());

        public static Sender Instance { get { return lazy.Value; } }

        private Sender()
        {
        }

        public void Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}
