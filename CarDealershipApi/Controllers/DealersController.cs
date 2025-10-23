using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarDealershipApi.Data; // Используем контекст
using CarDealershipApi.Models; // Используем модель Dealer

namespace CarDealershipApi.Controllers
{
    [Route("api/[controller]")] // Устанавливает маршрут: /api/Dealers
    [ApiController]
    public class DealersController : ControllerBase
    {
        private readonly CarDealershipContext _context;

        // Конструктор: EF Core автоматически внедрит контекст БД
        public DealersController(CarDealershipContext context)
        {
            _context = context;
        }

        // ЛР 3, Метод 1: Получение всех дилеров
        // GET: api/Dealers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dealer>>> GetDealers()
        {
            return await _context.Dealers.ToListAsync();
        }

        // ЛР 3, Метод 2: Получение дилера по ID
        // GET: api/Dealers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dealer>> GetDealer(int id)
        {
            var dealer = await _context.Dealers.FindAsync(id);

            if (dealer == null)
            {
                return NotFound(); // HTTP 404
            }

            return dealer; // HTTP 200 OK
        }

        // ЛР 4, Метод 1: Создание нового дилера
        // POST: api/Dealers
        [HttpPost]
        public async Task<ActionResult<Dealer>> PostDealer(Dealer dealer)
        {
            // 1. Добавляем объект в контекст
            _context.Dealers.Add(dealer);

            // 2. Сохраняем изменения в базе данных. 
            //    EF Core автоматически обновляет объект 'dealer' с присвоенным ID.
            await _context.SaveChangesAsync();

            // 3. Возвращаем ответ 201 Created. 
            //    Используем 'dealer.ID' (который теперь содержит новый ID) для ссылки на новый ресурс.
            return CreatedAtAction(nameof(GetDealer), new { id = dealer.ID }, dealer);
        }

        // ЛР 4, Метод 2: Обновление существующего дилера
        // PUT: api/Dealers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealer(int id, Dealer dealer)
        {
            if (id != dealer.ID)
            {
                return BadRequest(); // HTTP 400
            }

            _context.Entry(dealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Проверка, существует ли элемент с таким ID
                if (!_context.Dealers.Any(e => e.ID == id))
                {
                    return NotFound(); // HTTP 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // HTTP 204 No Content (успешное обновление)
        }

        // ЛР 4, Метод 3: Удаление дилера
        // DELETE: api/Dealers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealer(int id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return NotFound(); // HTTP 404
            }

            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204 No Content (успешное удаление)
        }
    }
}