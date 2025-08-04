using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace MessagingLibrary
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly string _connectionString;

        public RabbitMQConsumer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void StartConsuming(string queueName, Action<string> onMessageReceived)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived.Invoke(message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
