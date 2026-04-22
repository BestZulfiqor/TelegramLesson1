using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace TelegramBot.User.Pages;

public class EnterItPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        var text = @"Enter to It page welcoming you!";
        var markup = GetMarkup();
        // var photo = "https://images.unsplash.com/photo-1508921912186-1d1a45ebb3c1?w=500";
        // For videos use google drive

        var path = @"Resources/2026-04-09 14-58-00.mp4";

        var resources = ResourcesService.GetResourse(path);

        return new VideoPageResult(resources, text, markup)
        {
            UpdatedUserState = new UserState(this, userState.UserData)
        };

        // return new VideoPageResult(InputFile.FromFileId("BAACAgIAAxkBAAIChGnjibCwtOtMNtp5CZeT4ng-Emy1AAI2pwAC3zIhSyXcM2qKGqHeOwQ"), text, markup)
        // {
        //     UpdatedUserState = new UserState(this, userState.UserData)
        // };
    }

    public PageResultBase Update(Update update, UserState userState)
    {
        throw new NotImplementedException();
    }

    private InlineKeyboardMarkup GetMarkup()
    {
        return new InlineKeyboardMarkup([[InlineKeyboardButton.WithUrl("More", "https://images.unsplash.com/photo-1508921912186-1d1a45ebb3c1?w=500")]]);
    }
}
