using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class DocumentPageResult(InputFile document, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Document { get; set; } = document;
    }
}
