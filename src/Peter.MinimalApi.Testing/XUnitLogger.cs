using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Peter.MinimalApi.Testing;

public class XUnitLogger : ILogger
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly string _categoryName;

    public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
    {
        _testOutputHelper = testOutputHelper;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => NoopDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        try
        {
            var message = $"{_categoryName} [{eventId}]\n{formatter(state, exception)}";
            _testOutputHelper.WriteLine(message);
            if (exception is not null)
            {
                _testOutputHelper.WriteLine(exception.ToString());
            }
        }
        catch (InvalidOperationException)
        {
            // Prevent System.InvalidOperationException: 'There is no currently active test.'
            // BeforeAfterTestAttribute has an active test, but ICollectionFixture<> does not have
        }
    }

    private class NoopDisposable : IDisposable
    {
        public static readonly NoopDisposable Instance = new();

        public void Dispose() { }
    }
}