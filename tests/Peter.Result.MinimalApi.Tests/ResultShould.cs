using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class ResultShould
{
    private static readonly Fixture Fixture = new();

    [Fact]
    public void create_ok_result()
    {
        var result = OkResult<object>.Create();
        
        result.Success.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void create_ok_with_value()
    {
        var value = Fixture.Create<object>();
        
        var result = OkResult<object>.Create(value);
        
        result.Success.Should().BeTrue();
        result.Value.Should().Be(value);
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
        var value = Fixture.Create<object>();
        
        var result = NotFoundResult<object>.Create(value);
        
        result.Success.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void create_invalid_result()
    {
        var errors = new List<ValidationProblemError> { new(field: "Name", message: "Mandatory") };
        
        var result = InvalidResult<object>.Create(errors);
        
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var errors = new List<ValidationProblemError> { new(field: "Name", message: "Mandatory") };
        var value = Fixture.Create<object>();
        
        var result = InvalidResult<object>.Create(errors, value);
        
        result.Success.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Errors.Should().NotBeEmpty();
    }
}