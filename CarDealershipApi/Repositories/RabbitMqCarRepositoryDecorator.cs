public class RabbitMqCarRepositoryDecorator : ICarRepository
{
    private readonly ICarRepository _decorated;
    private readonly IEventPublisher _publisher;
    private const string ExchangeName = "cars_events_exchange";

    public RabbitMqCarRepositoryDecorator(ICarRepository decorated, IEventPublisher publisher)
    {
        _decorated = decorated;
        _publisher = publisher;
    }

    public async Task<Car> CreateAsync(Car car)
    {
        // 1. Выполняем основную логику (сохранение в БД)
        var createdCar = await _decorated.CreateAsync(car);

        // 2. Публикуем событие "CREATE"
        var carEvent = new CarEvent { EventType = "CREATE", Car = createdCar };
        var message = JsonConvert.SerializeObject(carEvent);
        _publisher.Publish(message);

        return createdCar;
    }

    public async Task UpdateAsync(Car car)
    {
        // 1. Выполняем основную логику (обновление в БД)
        await _decorated.UpdateAsync(car);

        // 2. Публикуем событие "UPDATE"
        var carEvent = new CarEvent { EventType = "UPDATE", Car = car };
        var message = JsonConvert.SerializeObject(carEvent);
        _publisher.Publish(message);
    }

    public async Task DeleteAsync(int id)
    {
        // Получаем объект, чтобы отправить его в событии перед удалением
        var carToDelete = await _decorated.GetByIdAsync(id);

        // 1. Выполняем основную логику (удаление из БД)
        await _decorated.DeleteAsync(id);

        // 2. Публикуем событие "DELETE"
        var carEvent = new CarEvent { EventType = "DELETE", Car = carToDelete };
        var message = JsonConvert.SerializeObject(carEvent);
        _publisher.Publish(message);
    }

    // Методы GET просто делегируются
    public Task<Car> GetByIdAsync(int id) => _decorated.GetByIdAsync(id);
    public Task<IEnumerable<Car>> GetAllAsync() => _decorated.GetAllAsync();
}