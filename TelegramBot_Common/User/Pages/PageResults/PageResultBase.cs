using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Common.User.Pages.PageResults
{
    public class PageResultBase(string text, ReplyMarkup replyMarkup)
    {
        public string Text { get; } = text;
        public ReplyMarkup ReplyMarkup { get; } = replyMarkup;
        public UserState UpdatedUserState { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.MarkdownV2;

        public bool IsMedia =>
            this is PhotoPageResult ||
            this is VideoPageResult ||
            this is DocumentPageResult ||
            this is AudioPageResult;
    }
}