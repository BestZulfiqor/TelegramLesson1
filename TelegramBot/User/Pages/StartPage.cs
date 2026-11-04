using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.User.Pages.Mozgokachalka;
using TelegramBot.User.Pages.PageResults;

namespace TelegramBot.User.Pages;

public class StartPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = Resources.StartPageText;

        var replyMarkup = GetInlineButtons();
        userState.AddPage(this);
        return new PageResultBase(text, replyMarkup)
        {
            UpdatedUserState = userState
        };
    }

    public PageResultBase Handle(Update update, UserState userState)
    {
        if (update.CallbackQuery is null)
        {
            return View(update, userState);
        }
        
        if (update.CallbackQuery.Data == "Мозгокачалка")
        {
            return new MozgokachalkaPage().View(update, userState);
        }

        if (update.CallbackQuery.Data == "Войти в айти")
        {
            return new EnterItPage().View(update, userState);
        }

        return View(update, userState);
    }

    private InlineKeyboardMarkup GetInlineButtons()
    {
        return new InlineKeyboardMarkup
        (   
            [
                [
                    InlineKeyboardButton.WithCallbackData("Мозгокачалка")
                ],
                [
                    InlineKeyboardButton.WithCallbackData("Войти в айти")
                ]
            ]);
    }
}
