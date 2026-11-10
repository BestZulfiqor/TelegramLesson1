using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_Common.Storage;
using TelegramBot_Common.User;
using TelegramBot_Common.User.Pages;
using TelegramBot_Common.User.Pages.PageResults;
using Message = TelegramBot_Common.User.Message;

namespace TelegramBot_Common;

public class UpdateHandler(UserStateStorage storage) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
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

        var userState = await storage.TryGetAsync(telegramUserId);
        if (userState == null)
        {
            userState = new UserState(new Stack<IPage>([new NotStartedPage()]), new UserData());
        }

        Console.WriteLine($"UpdateId={update.Id}, CURRENT_userState={userState}");

        var result = userState!.CurrentPage.Handle(update, userState);

        Console.WriteLine(
            $"UpdateId={update.Id}, send_text: {result.Text}, UpdatedUserState={result.UpdatedUserState}");
        var lastMessage = await SendResult(client, update, telegramUserId, result);

        result.UpdatedUserState.UserData.LastMessage = new Message(lastMessage.Id, result.IsMedia);
        await storage.AddOrUpdateAsync(telegramUserId, result.UpdatedUserState);
    }

    public async Task HandleErrorAsync(ITelegramBotClient client, Exception ex, HandleErrorSource source,
        CancellationToken
            token)
    {
        Console.WriteLine("Error: " + ex.Message);
    }

    private static async Task<Telegram.Bot.Types.Message> SendResult(ITelegramBotClient client, Update update,
        long telegramUserId, PageResultBase result)
    {
        switch (result)
        {
            case PhotoPageResult photoPageResult:
                return await SendPhoto(client, update, telegramUserId, photoPageResult);
            case VideoPageResult videoPageResult:
                return await SendVideo(client, update, telegramUserId, videoPageResult);
            default:
                return await SendText(client, update, telegramUserId, result);
        }
    }

    private static async Task<Telegram.Bot.Types.Message> SendPhoto(ITelegramBotClient client, Update update,
        long telegramUserId, PhotoPageResult result)
    {
        if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
        {
            return await client.EditMessageMedia(
                chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                media: new InputMediaPhoto(result.Photo)
                {
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

    private static async Task<Telegram.Bot.Types.Message> SendVideo(ITelegramBotClient client, Update update,
        long telegramUserId, VideoPageResult result)
    {
        if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
        {
            return await client.EditMessageMedia(
                chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                media: new InputMediaVideo(result.Video)
                {
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

    private static async Task<Telegram.Bot.Types.Message> SendText(ITelegramBotClient client, Update update,
        long telegramUserId, PageResultBase result)
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
            await client.DeleteMessage(chatId: telegramUserId,
                messageId: result.UpdatedUserState.UserData.LastMessage.Id);
        }

        return await client.SendMessage(
            chatId: telegramUserId,
            text: result.Text,
            replyMarkup: result.ReplyMarkup,
            parseMode: ParseMode.MarkdownV2,
            linkPreviewOptions: true);
    }
}