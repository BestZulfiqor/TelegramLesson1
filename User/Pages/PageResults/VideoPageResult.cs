using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class VideoPageResult(InputFile video, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Video { get; set; } = video;
    }
}
