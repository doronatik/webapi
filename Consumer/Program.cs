using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.RequestedHeartbeat = 5;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine($" Heartbear: {connection.Heartbeat}.");
                connection.RecoverySucceeded += Connection_RecoverySucceeded;
                connection.ConnectionShutdown += Connection_ConnectionShutdown;
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                consumer.Registered += Consumer_Registered;
                consumer.Unregistered += Consumer_Unregistered;
                consumer.Shutdown += Consumer_Shutdown;

                channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Consumer_Shutdown.");
        }

        private static void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine("Consumer_Unregistered.");
        }

        private static void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine("Consumer_Registered.");
        }

        private static void Connection_RecoverySucceeded(object sender, EventArgs e)
        {
            Console.WriteLine("Connection RecoverySucceeded.");
        }

        private static void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection shut down?.");
        }
    }
}
