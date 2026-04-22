using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace TelegramBot.User.Pages
{
    public class MozgokachalkaPage : IPage
    {
        public PageResultBase View(Update update, UserState userState)
        {
            var text = "Here you in Mozgokachalka";
            var replyMarkup = GetReplyKeyboard();
        
            var resource = ResourcesService.GetResourse("Resources/Images/IMG_1543.PNG");

            userState.AddPage(this);
            return new PhotoPageResult(photo: resource, text, replyMarkup)
            {
                UpdatedUserState = userState
            };
        }

        public PageResultBase Update(Update update, UserState userState)
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
