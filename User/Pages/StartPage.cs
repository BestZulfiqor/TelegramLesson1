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

    public PageResultBase Update(Update update, UserState userState)
    {
        if (update.CallbackQuery.Data == "Mozgokachalka")
        {
            return new MozgokachalkaPage().View(update, userState);
        }

        if (update.CallbackQuery.Data == "Voiti v It")
        {
            return new EnterItPage().View(update, userState);
        }

        return new PageResultBase("Press buttons", GetInlineButtons());
    }

    private InlineKeyboardMarkup GetInlineButtons()
    {
        return new InlineKeyboardMarkup
        (
            [
                [
                    InlineKeyboardButton.WithCallbackData("Mozgokachalka")
                ],
                [
                    InlineKeyboardButton.WithCallbackData("Voiti v It")
                ]
            ]);
    }
}
