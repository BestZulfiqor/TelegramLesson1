using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Common.User.Pages.PageResults
{
    public class AudioPageResult(InputFile audio, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Audio { get; set; } = audio;
    }
}
