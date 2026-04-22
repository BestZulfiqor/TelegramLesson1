using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages;

public class MozgokachalkaPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = "Here you in Mozgokachalka";
        var replyMarkup = GetReplyKeyboard();
        
        return new PhotoPageResult(photo: InputFile.FromFileId(""), text, replyMarkup)
        {
            UpdatedUserState = new UserState(this, userState.UserData)
        };
    }

    public PageResultBase Update(Update update, UserState userState)
    {
        if (update.CallbackQuery.Data == "Back")
        {
            return new StartPage().View(update, userState);
        }
        return null;
    }

    private InlineKeyboardMarkup GetReplyKeyboard()
    {
        return new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithCallbackData("Description"),
                InlineKeyboardButton.WithCallbackData("Watch live")
            ],
            [
                InlineKeyboardButton.WithCallbackData("Enter to group")
            ],
            [
                InlineKeyboardButton.WithCallbackData("Back")
            ]
        ]);
    }
}
