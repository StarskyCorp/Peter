using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_not_exist_result()
    {
        var result = NotExistResult<object>.Create();

        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_not_exist_result_with_value()
    {
        var value = Fixture.Create<object>();

        var result = NotExistResult<object>.Create(value);

        result.Success.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void create_invalid_result()
    {
        var errors = new List<ValidationProblemError> { new(identifier: "Name", message: "Mandatory") };

        var result = InvalidResult<object>.Create(errors);

        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var errors = new List<ValidationProblemError> { new(identifier: "Name", message: "Mandatory") };
        var value = Fixture.Create<object>();

        var result = InvalidResult<object>.Create(errors, value);

        result.Success.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Errors.Should().NotBeEmpty();
    }
}