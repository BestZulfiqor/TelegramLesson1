using Firebase.Database;
using Firebase.Database.Query;

namespace TelegramBot_Common.Firebase;

public class FirebaseProvider(FirebaseClient client)
{
    public async Task<T> TryGetAsync<T>(string key)
    {
        return await client.Child(key).OnceSingleAsync<T>();
    }
    
    public async Task AddOrUpdateAsync<T>(string key, T value)
    {
        await client.Child(key).PutAsync(value);
    }
}