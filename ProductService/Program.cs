using Microsoft.EntityFrameworkCore;
using MassTransit;
using ProductService.Data;
using ProductService.Consumers;

namespace ProductService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false);

            // Add services to the container.
            builder.Services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderFailedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbit = builder.Configuration.GetSection("RabbitMq");
                    cfg.Host(rabbit["Host"] ?? "localhost", h =>
                    {
                        h.Username(rabbit["Username"] ?? "guest");
                        h.Password(rabbit["Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("product-service", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(context);
                        e.ConfigureConsumer<OrderFailedConsumer>(context);
                    });
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
