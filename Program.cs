using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Storage;
using TelegramBot.User;
using TelegramBot.User.Pages;

class Program
{
    static UserStateStorage storage = new();
    static async Task Main(string[] args)
    {
        var telegramClient = new TelegramBotClient("8535196591:AAFLYX3Sm8qSe5oQw0WwZ4C0MQurqQmC65w");

        var user = await telegramClient.GetMe();
        Console.WriteLine($"Stating reveiving updates from user: {user.Username}");

        telegramClient.StartReceiving(updateHandler: HandleUpdate, errorHandler: HandleError);

        Console.ReadLine();
    }

    private static async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var allowedTypes = new HashSet<UpdateType>
        {
            UpdateType.Message,
            UpdateType.CallbackQuery
        };

        if (!allowedTypes.Contains(update.Type))
        {
            return;
        }

        long telegramUserId;
        if (update.Type == UpdateType.Message)
        {
            telegramUserId = update!.Message!.From!.Id;
        }
        else
        {
            telegramUserId = update.CallbackQuery!.From.Id;
        }

        Console.WriteLine($"Update={update.Id}, telegramUserId={telegramUserId}");

        var isExistUserState = storage.TryGet(telegramUserId, out var userState);
        if (!isExistUserState)
        {
            userState = new UserState(new NotStartedPage(), new UserData());
        }

        Console.WriteLine($"UpdateId={update.Id}, CURRENT_userState={userState}");

        var result = userState!.Page.Update(update, userState);

        Console.WriteLine($"UpdateId={update.Id}, send_text: {result.Text}, UpdatedUserState={result.UpdatedUserState}");
        await ChooseAnswer(client, update, telegramUserId, isExistUserState, result);

        storage.AddOrUpdate(telegramUserId, result.UpdatedUserState);
    }

    private static async Task ChooseAnswer(ITelegramBotClient client, Update update, long telegramUserId, bool isExistUserState, PageResultBase result)
    {
        switch (result)
        {
            case PhotoPageResult photoPageResult:
                await client.SendPhoto(
                    chatId: telegramUserId,
                    photo: photoPageResult.Photo,
                    caption: photoPageResult.Text,
                    replyMarkup: photoPageResult.ReplyMarkup
                );
                break;
            case VideoPageResult videoPageResult:
                await client.SendVideo(
                    chatId: telegramUserId,
                    video: videoPageResult.Video,
                    caption: videoPageResult.Text,
                    replyMarkup: videoPageResult.ReplyMarkup
                );
                break;
            case AudioPageResult audioPageResult:
                await client.SendAudio(
                    chatId: telegramUserId,
                    audio: audioPageResult.Audio,
                    caption: audioPageResult.Text,
                    replyMarkup: audioPageResult.ReplyMarkup
                );
                break;
            case DocumentPageResult documentPageResult:
                await client.SendDocument(
                    chatId: telegramUserId,
                    document: documentPageResult.Document,
                    caption: documentPageResult.Text,
                    replyMarkup: documentPageResult.ReplyMarkup
                );
                break;
            default:
                if (!isExistUserState)
                {
                    await client.SendMessage(
                        chatId: telegramUserId,
                        text: result.Text,
                        replyMarkup: result.ReplyMarkup,
                        parseMode: ParseMode.MarkdownV2,
                        linkPreviewOptions: true);
                }
                else
                {
                    await client.EditMessageText(
                        chatId: telegramUserId,
                        messageId: update.CallbackQuery.Message.Id,
                        text: result.Text,
                        replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup,
                        parseMode: ParseMode.MarkdownV2);
                }
                break;
        }
    }

    private static Task HandleError(ITelegramBotClient client, Exception ex, HandleErrorSource source, CancellationToken token)
    {
        Console.WriteLine("Error: " + ex.Message);
        return Task.CompletedTask;
    }
}

