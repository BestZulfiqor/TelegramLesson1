using Telegram.Bot.Types;

namespace TelegramBot.Services;

public class ResourcesService
{
    public static InputFileStream GetResourse(string path)
    {
        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return InputFile.FromStream(fileStream);
    }
}
