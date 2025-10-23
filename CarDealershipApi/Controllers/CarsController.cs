using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarDealershipApi.Data; // Используем контекст
using CarDealershipApi.Models; // Используем модель Car

namespace CarDealershipApi.Controllers
{
    [Route("api/[controller]")] // Устанавливает маршрут: /api/Cars
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarDealershipContext _context;

        public CarsController(CarDealershipContext context)
        {
            _context = context;
        }

        // ЛР 3, Метод 1: Получение всех автомобилей
        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            // Include(c => c.Dealer) загружает данные связанного дилера
            return await _context.Cars.Include(c => c.Dealer).ToListAsync();
        }

        // ЛР 3, Метод 2: Получение автомобиля по ID
        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.Include(c => c.Dealer).FirstOrDefaultAsync(c => c.ID == id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // ЛР 4, Метод 1: Создание нового автомобиля
        // POST: api/Cars
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            // Обязательная проверка: должен существовать дилер, к которому привязывается автомобиль
            var dealerExists = await _context.Dealers.AnyAsync(d => d.ID == car.DealerID);
            if (!dealerExists)
            {
                return BadRequest($"Dealer with ID {car.DealerID} does not exist."); // HTTP 400
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
        }

        // ЛР 4, Метод 2: Обновление существующего автомобиля
        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.ID)
            {
                return BadRequest();
            }

            // Обязательная проверка: убедиться, что новый DealerID существует
            if (car.DealerID != 0)
            {
                var dealerExists = await _context.Dealers.AnyAsync(d => d.ID == car.DealerID);
                if (!dealerExists)
                {
                    return BadRequest($"Dealer with ID {car.DealerID} does not exist.");
                }
            }


            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ЛР 4, Метод 3: Удаление автомобиля
        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}