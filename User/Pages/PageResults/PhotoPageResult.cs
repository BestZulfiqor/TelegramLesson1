using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.User.Pages
{
    public class PhotoPageResult(InputFile photo, string text, ReplyMarkup replyMarkup) : PageResultBase(text, replyMarkup)
    {
        public InputFile Photo { get; set; } = photo;
    }
}
