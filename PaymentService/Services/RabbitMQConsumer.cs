using Microsoft.AspNetCore.Connections;
using PaymentService.Data;
using PaymentService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PaymentService.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("orderQueue", false, false, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var order = JsonSerializer.Deserialize<Order>(message);

                if (order == null) return;

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Payments.Add(new Payment
                {
                    OrderId = order.OrderId,
                    Amount = order.Amount
                });

                context.SaveChanges();
                Console.WriteLine($"Processing payment for {order.OrderId}");

                // 👉 هنا تقدر تعمل Payment Logic
            };

            channel.BasicConsume("orderQueue", true, consumer);

            return Task.CompletedTask;
        }
    }
}
