using Telegram.Bot.Types;
using TelegramBot_Common.User.Pages.PageResults;

namespace TelegramBot_Common.User.Pages;

public interface IPage
{
    PageResultBase View(Update update, UserState userState);
    PageResultBase Handle(Update update, UserState userState);
}
