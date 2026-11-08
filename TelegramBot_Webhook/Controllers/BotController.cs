using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot.Configuration;

namespace TelegramBot_Webhook.Controllers;

[ApiController]
public class BotController(IUpdateHandler updateHandler, ITelegramBotClient botClient) : Controller
{
    [HttpPost(BotConfiguration.UpdateRoute)]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        try
        {
            Console.WriteLine("WEBHOOK RECEIVED");
            await updateHandler.HandleUpdateAsync(botClient, update, CancellationToken.None);
        }
        catch (Exception exception)
        {
            await updateHandler.HandleErrorAsync(botClient, exception, HandleErrorSource.HandleUpdateError,
                CancellationToken.None);
        }

        return Ok();
    }

    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok("Ok");
    }
}