using AutoFixture;
using FluentAssertions;

namespace Peter.Result.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_success_result()
    {
        var result = Result<object>.CreateOk(Fixture.Create<object>());
        result.Ok.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void create_success_result_without_value()
    {
        var result = Result<object>.CreateOk(value: null);
        result.Ok.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_failed_result()
    {
        var result = Result<object>.CreateError(Fixture.Create<IEnumerable<Error>>());
        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_failed_result_with_errors()
    {
        var result = Result<object>.CreateError(Fixture.Create<IEnumerable<Error>>());

        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_failed_result_with_errors_and_value()
    {
        var value = Fixture.Create<object>();

        var result = Result<object>.CreateError(value, Fixture.Create<IEnumerable<Error>>());

        result.Ok.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void convert_to_bool_from_successful_result()
    {
        var result = Result<object>.CreateOk(Fixture.Create<object>());

        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void convert_to_bool_from_failed_result()
    {
        var result = Result<object>.CreateError(Fixture.Create<IEnumerable<Error>>());

        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void convert_to_successful_result_from_any_value()
    {
        var value = Fixture.Create<string>();

        Result<string> result = value;

        result.Ok.Should().BeTrue();
        result.Value.Should().Be(value);
    }
}