using AutoFixture;
using FluentAssertions;

namespace Peter.Result.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_ok_result()
    {
        var result = OkResult<object>.Create();

        result.Ok.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_ok_result_with_value()
    {
        var result = OkResult<object>.Create(Fixture.Create<object>());

        result.Ok.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void create_error_result()
    {
        var result = ErrorResult<object>.Create();
        
        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void create_error_result_with_errors()
    {
        var result = ErrorResult<object>.Create(Fixture.Create<IEnumerable<Error>>());

        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_error_result_with_message()
    {
        var message = Fixture.Create<string>();

        var result = ErrorResult<object>.Create(message);

        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().BeEquivalentTo(new[] { new Error(message) });
    }

    [Fact]
    public void create_error_result_with_errors_and_value()
    {
        var value = Fixture.Create<object>();

        var result = ErrorResult<object>.Create(Fixture.Create<IEnumerable<Error>>(), value);

        result.Ok.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void convert_to_bool_from_ok_result()
    {
        var result = OkResult<object>.Create();

        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void convert_to_bool_from_error_result()
    {
        var result = ErrorResult<object>.Create();

        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void convert_to_ok_result_from_any_value()
    {
        var value = Fixture.Create<string>();

        OkResult<string> result = value;

        result.Ok.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void create_not_found_result()
    {
        var result = NotFoundResult<object>.Create();

        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_not_found_result_with_value()
    {
        var value = Fixture.Create<object>();

        var result = NotFoundResult<object>.Create(value);

        result.Ok.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void create_invalid_result()
    {
        var errors = new List<ValidationError> { new(identifier: "Name", message: "Mandatory") };

        var result = InvalidResult<object>.Create(errors);

        result.Ok.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ValidationErrors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var errors = new List<ValidationError> { new(identifier: "Name", message: "Mandatory") };
        var value = Fixture.Create<object>();

        var result = InvalidResult<object>.Create(errors, value);

        result.Ok.Should().BeFalse();
        result.Value.Should().Be(value);
        result.ValidationErrors.Should().NotBeEmpty();
    }
}