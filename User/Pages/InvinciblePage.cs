using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.User.Pages.PageResults;

namespace TelegramBot.User.Pages;

public class InvinciblePage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = @"The most popular moment from first seson of invincible\!";
        var markup = GetMarkup();
        userState.AddPage(this);
        return new VideoPageResult(InputFile.FromFileId("BAACAgIAAxkBAAIEQGrkAr3HDXHmFZOA1GdUwuCJk_mMAAJgrAACX0khS3tk6rJb0MxHPQQ"), text, 
            replyMarkup:  markup) 
        {
            UpdatedUserState = userState
        };
    }
    
    public PageResultBase Update(Update update, UserState userState)
    {
        if (update.CallbackQuery.Data == "Back")
        {
            userState.Pages.Pop();
            return userState.CurrentPage.View(update, userState);
        }

        return null;
    }
    
    private InlineKeyboardMarkup GetMarkup()
    {
        return new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithCallbackData("Back")
            ]
        ]);
    }
}
