namespace TelegramBot.User;

public class UserData
{
    public string StepikId { get; set; }
    public override string ToString()
    {
        return $"StepikId={StepikId}";
    }
}
