using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class PageResultBase(string text, ReplyMarkup replyMarkup)
    {
        public string Text { get; } = text;
        public ReplyMarkup ReplyMarkup { get; } = replyMarkup;
        public UserState UpdatedUserState { get; set; }
    }
}
