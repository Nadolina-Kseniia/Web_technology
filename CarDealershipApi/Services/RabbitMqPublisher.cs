public interface IEventPublisher
{
    void Publish(string message);
}

public class RabbitMqPublisher : IEventPublisher, IDisposable
{
    private const string ExchangeName = "cars_events_exchange";
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher()
    {
        // ”станавливаем соединение с Docker-контейнером
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        [cite_start]// ќбъ€вл€ем Exchange и очередь, а также создаем прив€зку (Binding) [cite: 296]
        _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);
        _channel.QueueDeclare(queue: "cars_events_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: "cars_events_queue", exchange: ExchangeName, routingKey: "car.event");
    }

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: ExchangeName,
                              routingKey: "car.event", // »спользуем routing key дл€ прив€зки
                              basicProperties: null,
                              body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}