using System.Reflection;
using TelegramBot_Common.User.Pages;

namespace TelegramBot_Common.Firebase;

public static class PagesFactory
{
    public static IPage GetPage(string pageName)
    {
        var pageType = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(t => t.Name == pageName && typeof(IPage).IsAssignableFrom(t));

        if (pageType == null)
        {
            throw new ArgumentException($"Page '{pageName}' not found.");
        }

        return (IPage)Activator.CreateInstance(pageType)!;
    }
}