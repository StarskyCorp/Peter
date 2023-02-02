﻿using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Peter.Testing;
using Xunit;

namespace Peter.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseCollectionFixture : ICollectionFixture<ServerFixture<Program, DatabaseCollectionInitializer>>
{
}

public class DatabaseCollectionInitializer : IPeterInitializer
{
    public void Initialize(IServiceProvider services)
    {
        //TODO: Initialize database or other stuff
    }
}

[Collection("Database collection")]
public class Collection_GreetingsApiShould
{
    private readonly ServerFixture<Program, DatabaseCollectionInitializer> _fixture;

    public Collection_GreetingsApiShould(ServerFixture<Program, DatabaseCollectionInitializer> fixture) => _fixture = fixture;

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        var response = await _fixture.Client().GetAsync("/Peter/Authenticated");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task greet_authenticated()
    {
        var claims = new Claim[] { new(ClaimTypes.Name, "Peter") };
        var response = await _fixture.AuthenticatedClient(claims).GetStringAsync("/Peter/Authenticated");
        response.Should().StartWith("Hello Peter!");
        response.Should().EndWith(string.Join<Claim>(",", claims));
    }
}