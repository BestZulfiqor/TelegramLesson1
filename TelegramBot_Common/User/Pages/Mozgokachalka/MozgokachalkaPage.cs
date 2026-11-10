using System.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_Common.Services;
using TelegramBot_Common.User.Pages.PageResults;
using TelegramBot;

namespace TelegramBot_Common.User.Pages.Mozgokachalka
{
    public class MozgokachalkaPage(ResourcesService resourcesService) : IPage
    {
        public PageResultBase View(Update update, UserState userState)
        {
            var text = "Here you in Mozgokachalka";
            var replyMarkup = GetReplyKeyboard();
            
            var resource = resourcesService.GetResource("TelegramBot_Common.Resources.Images.IMG_1543.PNG");            
            userState.AddPage(this);
            return new PhotoPageResult(photo: resource, text, replyMarkup)
            {
                UpdatedUserState = userState
            };
        }

        public PageResultBase Handle(Update update, UserState userState)
        {
            if (update.CallbackQuery.Data == "Enter to group")
            {
                return new MozgokachalkaRegisterPage().View(update, userState);
            }
            if (update.CallbackQuery.Data == "Back")
            {
                userState.Pages.Pop();
                return userState.CurrentPage.View(update, userState); 
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
}
