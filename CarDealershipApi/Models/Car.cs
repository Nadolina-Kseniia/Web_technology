namespace CarDealershipApi.Models
{
    public class Car
    {
        // Первичный ключ (ID)
        public int ID { get; set; }

        public string Firm { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Power { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }

        // Внешний ключ (DealerID)
        public int DealerID { get; set; }

        // Навигационное свойство для связи (Много автомобилей -> Один дилер)
        public Dealer Dealer { get; set; }
    }
}