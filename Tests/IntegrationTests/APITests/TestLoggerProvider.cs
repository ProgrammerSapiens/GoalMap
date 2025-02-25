using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.IntegrationTests.APITests
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _outputHelper;

        public TestLoggerProvider(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public ILogger CreateLogger(string categoryName) => new TestLogger(_outputHelper, categoryName);

        public void Dispose() { }
    }

    public class TestLogger : ILogger
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _categoryName;

        public TestLogger(ITestOutputHelper outputHelper, string categoryName)
        {
            _outputHelper = outputHelper;
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            _outputHelper.WriteLine($"[{logLevel}] {_categoryName}: {formatter(state, exception)}");
        }
    }
}
