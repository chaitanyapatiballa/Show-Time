using RabbitMQ.Client;
using System;
using System.Text;

namespace MessagingLibrary
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly string _connectionString;

        public RabbitMQPublisher(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Publish(string queueName, string message)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };

            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
