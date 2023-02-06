﻿using AutoFixture;
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
        result.Status.Should().Be(ResultStatus.Success);
    }

    [Fact]
    public void create_success_result_without_value()
    {
        var result = Result<object>.CreateSuccess(value: null);
        result.Success.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Success);
    }

    [Fact]
    public void create_failed_result()
    {
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<string>>());
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Failure);
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_failed_result_with_value()
    {
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<string>>(), Fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.Failure);
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_not_existing_result()
    {
        var result = Result<object>.CreateNotExists();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.NotExists);
    }

    [Fact]
    public void create_not_existing_result_with_value()
    {
        var result = Result<object>.CreateNotExists(Fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.NotExists);
    }

    [Fact]
    public void create_invalid_result()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = Result<object>.CreateInvalid(validationErrors);
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().NotBeEmpty();
    }

    [Fact]
    public void create_invalid_result_with_value()
    {
        var validationErrors = new List<ValidationError> { new(field: "Name", message: "Mandatory") };
        var result = Result<object>.CreateInvalid(validationErrors, Fixture.Create<object>());
        result.Success.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ValidationErrors.Should().NotBeEmpty();
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
        var result = Result<object>.CreateFailure(Fixture.Create<IEnumerable<string>>());
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
}