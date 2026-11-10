using Telegram.Bot.Types;
using TelegramBot_Common.User.Pages.PageResults;

namespace TelegramBot_Common.User.Pages;

public class NotStartedPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        return null;
    }
 
     public PageResultBase Handle(Update update, UserState userState)
    {
        return new StartPage().View(update, userState);
    }

}
