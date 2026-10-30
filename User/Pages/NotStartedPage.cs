using Telegram.Bot.Types;
using TelegramBot.User.Pages.PageResults;

namespace TelegramBot.User.Pages;

public class NotStartedPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        return null;
    }
 
     public PageResultBase Update(Update update, UserState userState)
    {
        return new StartPage().View(update, userState);
    }

}
