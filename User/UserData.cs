namespace TelegramBot.User;

public class UserData
{
    public string? StepikId { get; set; }
    public Message? LastMessage { get; set; }
    public override string ToString()
    {
        return $"StepikId={StepikId}";
    }
}
