using Microsoft.EntityFrameworkCore;
using CarDealershipApi.Models;

namespace CarDealershipApi.Data
{
    public class CarDealershipContext : DbContext
    {
        public CarDealershipContext(DbContextOptions<CarDealershipContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Dealer> Dealers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка точности Decimal для соответствия столбцам MySQL DECIMAL(10, 2)
            modelBuilder.Entity<Car>().Property(p => p.Price).HasColumnType("decimal(10, 2)");
            modelBuilder.Entity<Dealer>().Property(p => p.Rating).HasColumnType("decimal(10, 2)");

            // Настройка внешнего ключа (One-to-Many)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Dealer) // Один дилер
                .WithMany(d => d.Cars) // Много машин
                .HasForeignKey(c => c.DealerID); // Поле внешнего ключа

            // Указываем, что таблицы уже существуют и не нужно создавать миграции
            modelBuilder.Entity<Car>().ToTable("cars");
            modelBuilder.Entity<Dealer>().ToTable("dealers");
        }
    }
}