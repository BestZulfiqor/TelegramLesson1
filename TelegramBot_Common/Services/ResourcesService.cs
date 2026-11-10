using System.Reflection;
using Telegram.Bot.Types;

namespace TelegramBot_Common.Services;

public class ResourcesService
{
    public InputFileStream GetResource(byte[] buffer, string fileName)
    {
        var memoryStream = new MemoryStream(buffer);
        return InputFile.FromStream(memoryStream, fileName);
    }
}