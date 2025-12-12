using MassTransit;
using EmailService.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCompletedConsumer>();
    x.AddConsumer<OrderFailedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbit = builder.Configuration.GetSection("RabbitMq");
        cfg.Host(rabbit["Host"], h =>
        {
            h.Username(rabbit["Username"] ?? "guest");
            h.Password(rabbit["Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("email-service", e =>
        {
            e.ConfigureConsumer<OrderCompletedConsumer>(context);
            e.ConfigureConsumer<OrderFailedConsumer>(context);
        });
    });
});

var app = builder.Build();
app.Run();