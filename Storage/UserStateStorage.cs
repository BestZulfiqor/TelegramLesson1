using System.Collections.Concurrent;
using TelegramBot.User;

namespace TelegramBot.Storage;

public class UserStateStorage
{
    private readonly ConcurrentDictionary<long, UserState> cache = new ConcurrentDictionary<long, UserState>();

    public void AddOrUpdate(long telegramUserId, UserState state)
    {
        cache.AddOrUpdate(telegramUserId, state, (x, y) => state);
    }

    public bool TryGet(long telegramUserId, out UserState? userState)
    {
        return cache.TryGetValue(telegramUserId, out userState);
    }
}
