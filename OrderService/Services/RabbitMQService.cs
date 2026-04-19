using RabbitMQ.Client;
using System.Text;

namespace OrderService.Services
{
    public class RabbitMQService
    {
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orderQueue", false, false, false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "orderQueue", null, body);
        }
    }
}
