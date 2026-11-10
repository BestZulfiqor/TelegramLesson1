using TelegramBot_Common.Firebase;
using TelegramBot_Common.User;
using TelegramBot_Common.User.Pages;

namespace TelegramBot_Common.Storage;

public class UserStateStorage(FirebaseProvider firebaseProvider) 
{

    public async Task AddOrUpdateAsync(long telegramUserId, UserState state)
    {
        var userStateFirebase = ToUserStateFirebase(state);
        await firebaseProvider.AddOrUpdateAsync("userStates/" + telegramUserId, userStateFirebase);
    }

    private static UserStateFirebase ToUserStateFirebase(UserState state)
    {
        return new UserStateFirebase()
        {
            UserData = state.UserData,
            PageNames = state.Pages.Select(x => x.GetType().Name).ToList()
        };
    }

    public async Task<UserState?> TryGetAsync(long telegramUserId)
    {
        var userStateFirebase = await firebaseProvider.TryGetAsync<UserStateFirebase>($"userStates/{telegramUserId}");
        if (userStateFirebase == null)
        {
            return null;
        }

        return ToUserState(userStateFirebase);
    }

    private static UserState? ToUserState(UserStateFirebase userStateFirebase)
    {
        var pages = userStateFirebase.PageNames.Select(PagesFactory.GetPage).Reverse();
        return new UserState(new Stack<IPage>(pages), userStateFirebase.UserData);
    }
}
