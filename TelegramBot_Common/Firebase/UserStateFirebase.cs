using TelegramBot_Common.User;

namespace TelegramBot_Common.Firebase;

public class UserStateFirebase
{
    public UserData UserData { get; set; }
    public List<string> PageNames { get; set; }
}