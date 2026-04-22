using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class MozgokachalkaRegisterPage : IPage
    {
        public PageResultBase View(Update update, UserState userState)
        {
            var text = @"Please send me your stepik id\. You can find it in your profile in Stepik site";
            var replyMarkup = GetReplyKeyboard();
        
            userState.AddPage(this);
            return new PageResultBase(text, replyMarkup)
            {
                UpdatedUserState = userState
            };
        }

        public PageResultBase Update(Update update, UserState userState)
        {
            if (update.Message != null)
            {
                userState.UserData.StepikId = update.Message.Text;
                return new MozgokachalkaRegisteredPage().View(update, userState);
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
                    InlineKeyboardButton.WithCallbackData("Back")
                ]
            ]);
        }
    }
}
