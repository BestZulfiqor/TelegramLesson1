using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace TelegramBot.User.Pages;

public class EnterItPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = @"Enter to It page welcoming you\!";
        var markup = GetMarkup();

        // var path = @"Resources/2026-04-09 14-58-00.mp4";

        // var resources = ResourcesService.GetResourse(path);

        // return new VideoPageResult(resources, text, markup)
        // {
        //     UpdatedUserState = new UserState(this, userState.UserData)
        // };

        userState.AddPage(this);
        return new VideoPageResult(InputFile.FromFileId("BAACAgIAAxkBAAIChGnjibCwtOtMNtp5CZeT4ng-Emy1AAI2pwAC3zIhSyXcM2qKGqHeOwQ"), text, markup)
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
                InlineKeyboardButton.WithUrl("More", "https://images.unsplash.com/photo-1508921912186-1d1a45ebb3c1?w=500")
            ],
            [
                InlineKeyboardButton.WithCallbackData("Back")
            ]
        ]);
    }
}
