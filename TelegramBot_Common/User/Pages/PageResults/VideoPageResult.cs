using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Common.User.Pages.PageResults
{
    public class VideoPageResult(InputFile video, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Video { get; set; } = video;
    }
}
