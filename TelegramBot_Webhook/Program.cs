using TelegramBot_Common;
using TelegramBot_Webhook;

var builder = WebApplication.CreateBuilder(args);
ContainerConfigurator.Configure(builder.Configuration, builder.Services);

builder.Services.AddHostedService<WebhookConfigurator>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();