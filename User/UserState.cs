using TelegramBot.User.Pages;

namespace TelegramBot.User;

public record class UserState(IPage Page, UserData UserData);