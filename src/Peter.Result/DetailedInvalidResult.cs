namespace Peter.Result;

public class DetailedInvalidResult<T> : InvalidResult<T>
{
    public IEnumerable<DetailedMessage> Details { get; }

    public DetailedInvalidResult(IEnumerable<DetailedMessage> details)
    {
        ArgumentNullException.ThrowIfNull(details);
        if (!details.Any())
        {
            throw new ArgumentException($"{nameof(details)} cannot be empty.");
        }

        Details = details;
    }

    public DetailedInvalidResult(string key, string message) : this(new[]
        { new DetailedMessage(key, new[] { message }) })
    {
    }
}