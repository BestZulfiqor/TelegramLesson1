using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_Common.User;
using TelegramBot_Common.User.Pages;
using TelegramBot_Common.User.Pages.Mozgokachalka;
using TelegramBot_Common.User.Pages.PageResults;
using Message = Telegram.Bot.Types.Message;

namespace TelegramBot.Tests;

public class StartPageTests
{
    [Test]
    public void View_FirstEnter_CorrectTextAndKeyboard()
    {
        // Arrange
        var startPage = new StartPage();
        var pages = new Stack<IPage>([new NotStartedPage()]);
        var userState = new UserState(pages, new UserData());
        var expectedButtons = new InlineKeyboardButton[][]
        {
            [InlineKeyboardButton.WithCallbackData("Мозгокачалка")],
            [InlineKeyboardButton.WithCallbackData("Войти в айти")]
        };

        // Act
        var result = startPage.View(null, userState);

        // Assert
        Assert.That(result, Is.InstanceOf<PageResultBase>());
        Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
        Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(startPage));
        Assert.That(result.Text, Is.EqualTo(Resources.StartPageText));
        Assert.That(result.ParseMode, Is.EqualTo(ParseMode.MarkdownV2));
        Assert.That(result.ReplyMarkup, Is.InstanceOf<InlineKeyboardMarkup>());
        KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
    }

    [Test]
    public void Handle_MozgokachalkaCallback_MozgokachalkaPage()
    {
        // Arrange
        var startPage = new StartPage();
        var pages = new Stack<IPage>([new NotStartedPage(), startPage]);
        var userState = new UserState(pages, new UserData());
        var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Мозгокачалка" } };

        // Act
        var result = startPage.Handle(update, userState);

        // Assert
        Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));
        Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        Assert.That(result.UpdatedUserState.CurrentPage, Is.InstanceOf<MozgokachalkaPage>());
    }

    [Test]
    public void Handle_EnterToItPageCallback_EnterItPagePage()
    {
        // Arrange
        var startPage = new StartPage();
        var pages = new Stack<IPage>([new NotStartedPage(), startPage]);
        var userState = new UserState(pages, new UserData());
        var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Войти в айти" } };

        // Act
        var result = startPage.Handle(update, userState);

        // Assert
        Assert.That(result.GetType(), Is.EqualTo(typeof(VideoPageResult)));
        Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        Assert.That(result.UpdatedUserState.CurrentPage, Is.InstanceOf<EnterItPage>());
    }

    [Test]
    public void Handle_UnknownMessage_StartPageView()
    {
        // Arrange
        var startPage = new StartPage();
        var pages = new Stack<IPage>([new NotStartedPage()]);
        var userState = new UserState(pages, new UserData());
        var update = new Update { Message = new Message { Text = "Неверный текст" } };
        var expectedButtons = new InlineKeyboardButton[][]
        {
            [InlineKeyboardButton.WithCallbackData("Мозгокачалка")],
            [InlineKeyboardButton.WithCallbackData("Войти в айти")]
        };

        // Act
        var result = startPage.View(update, userState);

        // Assert
        Assert.That(result, Is.InstanceOf<PageResultBase>());
        Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
        Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(startPage));
        Assert.That(result.Text, Is.EqualTo(Resources.StartPageText));
        Assert.That(result.ParseMode, Is.EqualTo(ParseMode.MarkdownV2));
        Assert.That(result.ReplyMarkup, Is.InstanceOf<InlineKeyboardMarkup>());
        KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
    }
}