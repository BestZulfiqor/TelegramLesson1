using Microsoft.AspNetCore.Mvc.RazorPages;
using Telegram.Bot.Types;
using TelegramBot.User.Pages.PageResults;

namespace TelegramBot.User.Pages;

public interface IPage
{
    PageResultBase View(Update update, UserState userState);
    PageResultBase Handle(Update update, UserState userState);
}
