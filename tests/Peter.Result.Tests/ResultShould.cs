using AutoFixture;
using FluentAssertions;

namespace Peter.Result.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_success_result()
    {
        var result = Result<object>.CreateSuccess(Fixture.Create<object>());
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
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<Error>>());
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_failed_result_with_value()
    {
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<Error>>(), Fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_not_found_result()
    {
        var result = NotFoundResult<object>.Create();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_not_found_result_with_value()
    {
        var result = NotFoundResult<object>.Create(Fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void create_invalid_result()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = InvalidResult<object>.Create(validationErrors);
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = InvalidResult<object>.Create(validationErrors, "foo");
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_validation_result()
    {
        var errors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = ValidationResult.Create(false, errors);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void convert_to_bool_from_successful_result()
    {
        var result = Result<object>.CreateSuccess(Fixture.Create<object>());
        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void convert_to_bool_from_failed_result()
    {
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<Error>>());
        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void convert_to_successful_result_from_any_value()
    {
        var value = Fixture.Create<string>();
        Result<string> result = value;
        result.Success.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void fail_when_creating_a_non_success_validation_result_with_no_validation_errors()
    {
        Action act = () => ValidationResult.Create(false, null);
        act.Should().Throw<ArgumentNullException>();
    }
}