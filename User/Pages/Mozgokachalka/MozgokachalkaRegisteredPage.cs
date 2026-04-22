using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class MozgokachalkaRegisteredPage : IPage
    {
        public PageResultBase View(Update update, UserState userState)
        {
            var text = $@"Your stepik id is: {userState.UserData.StepikId}";
        
            userState.AddPage(this);
            return new PageResultBase(text, GetReplyKeyboard())
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

        private InlineKeyboardMarkup GetReplyKeyboard()
        {
            return new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("Back")
                ]
            ]);
        }
    }
}
