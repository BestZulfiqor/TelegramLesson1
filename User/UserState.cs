using TelegramBot.User.Pages;

namespace TelegramBot.User;

public record class UserState(Stack<IPage> Pages, UserData UserData)
{
    public IPage CurrentPage => Pages.Peek();

    /// <summary>
    /// Добавляет страницу в стек только если страница того же типа ещё не находится в стеке
    /// </summary>
    public void AddPage(IPage page)
    {
        if (!Pages.Any(p => p.GetType() == page.GetType()))
        {
            Pages.Push(page);
        }
    }
}