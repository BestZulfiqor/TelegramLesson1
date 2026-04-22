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
            userState = new UserState(new Stack<IPage>( [new NotStartedPage()]), new UserData());
        }

        Console.WriteLine($"UpdateId={update.Id}, CURRENT_userState={userState}");

        var result = userState!.CurrentPage.Update(update, userState);

        Console.WriteLine($"UpdateId={update.Id}, send_text: {result.Text}, UpdatedUserState={result.UpdatedUserState}");
        var lastMessage = await SendResult(client, update, telegramUserId, isExistUserState, result);

        result.UpdatedUserState.UserData.LastMessage = new TelegramBot.User.Message(lastMessage.Id, result.IsMedia);
        storage.AddOrUpdate(telegramUserId, result.UpdatedUserState);
    }

    private static async Task<Telegram.Bot.Types.Message> SendResult(ITelegramBotClient client, Update update, long telegramUserId, bool isExistUserState, PageResultBase result)
    {
        switch (result)
        {
            case PhotoPageResult photoPageResult:
                return await SendPhoto(client, update, telegramUserId, photoPageResult);
            case VideoPageResult videoPageResult:
                return await SendVideo(client, update, telegramUserId, videoPageResult);
            default:
                return await SendText(client, update, telegramUserId, isExistUserState, result);
        }
    }

    private static async Task<Telegram.Bot.Types.Message> SendPhoto(ITelegramBotClient client, Update update, long telegramUserId, PhotoPageResult result)
    {
        if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false)){
            return await client.EditMessageMedia(
                chatId: telegramUserId, 
                messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                media: new InputMediaPhoto(result.Photo){
                    Caption = result.Text,
                    ParseMode = ParseMode.MarkdownV2
                },
                replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup
            );
        }
        
        if (result.UpdatedUserState.UserData.LastMessage != null)
        {
            await client.DeleteMessage(
                chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage.Id);
        }

        return await client.SendPhoto(
                chatId: telegramUserId,
                photo: result.Photo,
                caption: result.Text,
                replyMarkup: result.ReplyMarkup
        );
    }

    private static async Task<Telegram.Bot.Types.Message> SendVideo(ITelegramBotClient client, Update update, long telegramUserId, VideoPageResult result)
    {
        if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false)){
            return await client.EditMessageMedia(
                chatId: telegramUserId, 
                messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                media: new InputMediaVideo(result.Video){
                    Caption = result.Text,
                    ParseMode = ParseMode.MarkdownV2
                },
                replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup
            );
        }
        
        if (result.UpdatedUserState.UserData.LastMessage != null)
        {
            await client.DeleteMessage(
                chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage.Id);
        }

        return await client.SendVideo(
                chatId: telegramUserId,
                video: result.Video,
                parseMode: ParseMode.MarkdownV2,
                caption: result.Text,
                replyMarkup: result.ReplyMarkup
        );
    }

    private static async Task<Telegram.Bot.Types.Message> SendText(ITelegramBotClient client, Update update, long telegramUserId, bool isExistUserState, PageResultBase result)
    {
        if (update.CallbackQuery != null && (!result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
        {
            return await client.EditMessageText(
                chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                text: result.Text,
                replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup,
                parseMode: ParseMode.MarkdownV2);
        }
        if (result.UpdatedUserState.UserData.LastMessage != null)
        {
            await client.DeleteMessage(chatId: telegramUserId, messageId: result.UpdatedUserState.UserData.LastMessage.Id);
        }

        return await client.SendMessage(
            chatId: telegramUserId,
            text: result.Text,
            replyMarkup: result.ReplyMarkup,
            parseMode: ParseMode.MarkdownV2,
            linkPreviewOptions: true);
    }

    private static Task HandleError(ITelegramBotClient client, Exception ex, HandleErrorSource source, CancellationToken token)
    {
        Console.WriteLine("Error: " + ex.Message);
        return Task.CompletedTask;
    }
}

