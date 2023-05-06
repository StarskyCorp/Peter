using AutoFixture;
using FluentAssertions;

namespace Peter.Result.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_an_ok_result_without_a_value()
    {
        var result = new OkResult<object>();

        result.Ok.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_an_ok_result_with_a_value()
    {
        var value = Fixture.Create<object>();

        var result = new OkResult<object>(value);

        result.Ok.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void convert_to_true_an_ok_result()
    {
        var result = new OkResult<object>();

        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void convert_to_an_ok_result_any_value()
    {
        var value = Fixture.Create<string>();

        OkResult<string> result = value;

        result.Ok.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void create_an_error_result_without_a_message()
    {
        var result = new ErrorResult<object>();

        result.Ok.Should().BeFalse();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void create_an_error_result_with_a_message()
    {
        var message = Fixture.Create<string>();

        var result = new ErrorResult<object>(message);

        result.Ok.Should().BeFalse();
        result.Error.Should().Be(message);
    }

    [Fact]
    public void convert_to_false_an_error_result()
    {
        var result = new ErrorResult<object>();

        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void create_a_not_found_result()
    {
        new NotFoundResult<object>().Ok.Should().BeFalse();
    }

    [Fact]
    public void create_an_invalid_result()
    {
        new InvalidResult<object>().Ok.Should().BeFalse();
    }

    [Fact]
    public void create_a_simple_invalid_result()
    {
        var message = Fixture.Create<string>();

        var result = new SimpleInvalidResult<object>(message);

        result.Ok.Should().BeFalse();
        result.Message.Should().Be(message);
    }

    [Fact]
    public void create_a_detailed_invalid_result()
    {
        var details = Fixture.Create<IEnumerable<DetailedMessage>>();

        var result = new DetailedInvalidResult<object>(details);

        result.Ok.Should().BeFalse();
        result.Details.Should().BeEquivalentTo(details);
    }

    [Fact]
    public void create_a_detailed_invalid_result_with_only_one_detail()
    {
        var key = Fixture.Create<string>();
        var message = Fixture.Create<string>();

        var result = new DetailedInvalidResult<object>(key, message);

        result.Ok.Should().BeFalse();
        result.Details.Should().BeEquivalentTo(new[] { new DetailedMessage(key, new[] { message }) });
    }
}