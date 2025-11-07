using System.Collections.Generic;
using System.Threading.Tasks;
using CarDealershipApi.Models;

public interface ICarRepository
{
    // Методы для чтения (READ)
    Task<Car> GetByIdAsync(int id);
    Task<IEnumerable<Car>> GetAllAsync();

    // Методы для модификации данных (CREATE, UPDATE, DELETE), 
    // после которых нужно публиковать события
    Task<Car> CreateAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(int id);
}