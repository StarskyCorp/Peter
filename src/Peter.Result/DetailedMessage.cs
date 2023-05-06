namespace Peter.Result;

public class DetailedMessage
{
    public string Key { get; }
    public IEnumerable<string> Messages { get; }

    public DetailedMessage(string key, IEnumerable<string> messages)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        Key = key;
        ArgumentNullException.ThrowIfNull(messages);
        if (!messages.Any())
        {
            throw new ArgumentException($"{nameof(messages)} cannot be empty.");
        }

        Messages = messages;
    }
}