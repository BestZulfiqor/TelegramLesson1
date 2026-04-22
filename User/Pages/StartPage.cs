using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages;

public class StartPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = @"*Hello, I'm glad to see you*
_Welcome to online school IRON PROGRAMMER\!_

__Do you have any question? Looking for IT path or want to join to Brainstorm?__

~Is this course interesting for you or you want to higher one?~

||Choose your point in menu or just write your question and I necessarily answer to your question and help you||

[Link](https://github.com/TelegramBots/Telegram.Bot)

`Mono text`

```csharp
return new PageResult(text, replyMarkup)
{   
    UpdateUserState = new UserState(this, userState.UserData)
};
```
";

        var replyMarkup = GetInlineButtons();

        return new PageResultBase(text, replyMarkup)
        {
            UpdatedUserState = new UserState(this, userState.UserData)
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
