using Firebase.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot_Common.Configuration;
using TelegramBot_Common.Firebase;
using TelegramBot_Common.Services;
using TelegramBot_Common.Storage;
namespace TelegramBot_Common;

public class ContainerConfigurator
{
    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        var firebaseConfigurationSection = configuration.GetSection(FirebaseConfiguration.SectionName);
        services.Configure<FirebaseConfiguration>(firebaseConfigurationSection);

        var botConfigurationSection = configuration.GetSection(BotConfiguration.SectionName);
        services.Configure<BotConfiguration>(botConfigurationSection);

        services.AddSingleton<UserStateStorage>();
        services.AddSingleton<FirebaseProvider>();
        services.AddSingleton<FirebaseClient>(services =>
        {
            var firebaseConfig = services.GetService<IOptions<FirebaseConfiguration>>()!.Value;
            return new FirebaseClient(firebaseConfig.BasePath, new FirebaseOptions()
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseConfig.Secret)
            });
        });

        services.AddHttpClient("tg_bot_client").AddTypedClient<ITelegramBotClient>((httpClient, services) =>
        {
            var botConfig = services.GetService<IOptions<BotConfiguration>>()!.Value;
            var options = new TelegramBotClientOptions(botConfig.BotToken);
            return new TelegramBotClient(options, httpClient);
        });

        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<ResourcesService>();
    }
}