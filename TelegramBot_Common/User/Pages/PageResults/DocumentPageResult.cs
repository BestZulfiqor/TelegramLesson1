using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Common.User.Pages.PageResults
{
    public class DocumentPageResult(InputFile document, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Document { get; set; } = document;
    }
}
