using Microsoft.AspNetCore.Mvc.RazorPages;
using Telegram.Bot.Types;

namespace TelegramBot.User.Pages;

public interface IPage
{
    PageResultBase View(Update update, UserState userState);
    PageResultBase Update(Update update, UserState userState);
}
