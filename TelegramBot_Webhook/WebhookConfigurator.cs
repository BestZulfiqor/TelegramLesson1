using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Configuration;

namespace TelegramBot_Webhook;

public class WebhookConfigurator(ITelegramBotClient botClient, IOptions<BotConfiguration> botConfiguration) : 
    IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var webhookAddress = botConfiguration.Value.HostAddress + BotConfiguration.UpdateRoute;
        await botClient.SetWebhook(
            url: webhookAddress,
            secretToken: botConfiguration.Value.SecretToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await botClient.DeleteWebhook();
    }
}