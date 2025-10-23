namespace CarDealershipApi.Models
{
    public class Dealer
    {
        // Первичный ключ (ID)
        public int ID { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public decimal Rating { get; set; } // DECIMAL(10, 2) в БД [cite: 199]

        // Навигационное свойство для связи (Один дилер -> Много автомобилей)
        public ICollection<Car>? Cars { get; set; }
    }
}