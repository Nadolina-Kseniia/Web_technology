using CarDealershipApi.Data; // Путь к DbContext
using CarDealershipApi.Models; // Путь к модели Car
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CarRepository : ICarRepository
{
    private readonly CarDealershipContext _context;

    public CarRepository(CarDealershipContext context)
    {
        // Контекст внедряется через DI
        _context = context;
    }

    public async Task<Car> CreateAsync(Car car)
    {
        _context.Cars.Add(car); // Логика создания
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task UpdateAsync(Car car)
    {
        _context.Entry(car).State = EntityState.Modified; // Логика обновления
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car); // Логика удаления
            await _context.SaveChangesAsync();
        }
    }

    // Методы чтения
    public async Task<Car> GetByIdAsync(int id) => await _context.Cars.FindAsync(id);
    public async Task<IEnumerable<Car>> GetAllAsync() => await _context.Cars.ToListAsync();
}