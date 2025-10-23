using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using CarDealershipApi.Data; // указывает на папку Data с контекстом

var builder = WebApplication.CreateBuilder(args);

// 1. Добавление сервисов контроллеров
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. Добавление поддержки Swagger/OpenAPI (для тестирования API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Настройка Entity Framework Core с провайдером MySQL
// Строка подключения из ЛР 1,2: "Server=localhost;Database=car_dealership;Uid=root;Pwd=root;"
var connectionString = "Server=localhost;Database=car_dealership;Uid=root;Pwd=root;";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31)); // !!! УКАЖИТЕ ВАШУ ВЕРСИЮ MySQL !!!

builder.Services.AddDbContext<CarDealershipContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();