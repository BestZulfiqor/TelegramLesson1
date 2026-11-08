using TelegramBot_Webhook;
using TelegramBot;

var builder = WebApplication.CreateBuilder(args);
ContainerConfigurator.Configure(builder.Configuration, builder.Services);

builder.Services.AddHostedService<WebhookConfigurator>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();