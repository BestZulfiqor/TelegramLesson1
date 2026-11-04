using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Tests;

public class KeyboardHelper
{
    public static void AssertKeyboard(InlineKeyboardButton[][] expectedButtons, InlineKeyboardMarkup actualKeyboard)
    {
        var actualButtons = actualKeyboard?.InlineKeyboard;
        Assert.That(actualButtons, Is.Not.Null);
        for (int i = 0; i < expectedButtons.Length; i++)
        {
            Assert.That(actualButtons.ElementAt(i).Count(), Is.EqualTo(expectedButtons[i].Length), $"Количество " + 
                $"кнопок в ряду {i} не совподает");
            for (int j = 0; j < expectedButtons[i].Length; j++)
            {
                var expectedButton = expectedButtons[i][j];
                var actualButton = actualButtons.ElementAt(i).ElementAt(j);
                
                Assert.That(actualButton.Text, Is.EqualTo(expectedButton.Text), $"Текст кнопки {i}, {j} не совпадает");
                Assert.That(actualButton.CallbackData, Is.EqualTo(expectedButton.CallbackData));
                Assert.That(actualButton.Url, Is.EqualTo(expectedButton.Url));
            }
        }
    }
}