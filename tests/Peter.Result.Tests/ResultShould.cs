using AutoFixture;
using FluentAssertions;

namespace Peter.Result.Tests;

public class ResultShould
{
    private static readonly Fixture _fixture = new();

    [Fact]
    public void create_success_result()
    {
        var result = Result<object>.CreateSuccess(_fixture.Create<object>());
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void create_success_result_without_value()
    {
        var result = Result<object>.CreateSuccess(value: null);
        result.Success.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_failed_result()
    {
        var result = Result<object>.CreateFailure(_fixture.Create<IEnumerable<string>>());
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_failed_result_with_value()
    {
        var result = Result<object>.CreateFailure(_fixture.Create<IEnumerable<string>>(), _fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_not_existing_result()
    {
        var result = NotFoundResult<object>.Create();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_not_existing_result_with_value()
    {
        var result = NotFoundResult<object>.Create(_fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void create_invalid_result()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = ValidationResult<object>.Create(validationErrors);
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ValidationErrors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = ValidationResult<object>.Create(validationErrors, _fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.ValidationErrors.Should().NotBeEmpty();
    }

    [Fact]
    public void convert_to_bool_from_successful_result()
    {
        var result = Result<object>.CreateSuccess(_fixture.Create<object>());
        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void convert_to_bool_from_failed_result()
    {
        var result = Result<object>.CreateFailure(_fixture.Create<IEnumerable<string>>());
        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void convert_to_successful_result_from_any_value()
    {
        var value = _fixture.Create<string>();
        Result<string> result = value;
        result.Success.Should().BeTrue();
        result.Value.Should().Be(value);
    }
}