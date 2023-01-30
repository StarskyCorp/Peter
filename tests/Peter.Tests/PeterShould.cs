using FluentAssertions;
using Xunit;

namespace Peter.Tests;

public class PeterShould
{
    [Fact]
    public void greet()
    {
        var peter = new Me();
        peter.Hello("Sergio").Should().Be("Hello Sergio, I'm Peter.");
    }
}