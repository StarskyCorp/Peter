﻿using Api;
using Api.Authentication;
using Api.Validation;
using FluentValidation;
using Peter.MinimalApi.Modules;
using Peter.MinimalApi.Validation;
using Void = Peter.Result.Void;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();

var app = builder.Build();

app.ConfigureToMinimalApi(options =>
{
    options.Configuration.AddNullType(typeof(Void));

    // open
    options.Configuration.RegisterCustomHandler(typeof(TeapotResult<>), result =>
    {
        var teapotResult = (TeapotResult<int>)result;
        return Results.Content($"I'm a {teapotResult.Value} teapot years old",
            statusCode: 418);
    });

    // closed
    options.Configuration.RegisterCustomHandler(typeof(TeapotResult<string>), result =>
    {
        var teapotResult = (TeapotResult<string>)result;
        return Results.Content($"I'm {(!teapotResult.Ok ? "not " : "")}{teapotResult.Value}'s teapot",
            statusCode: 418);
    });
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// The prefix can also be empty.
// This can be useful for adding endpoint metadata or filters to a group of endpoints without changing the route pattern
var all = app.MapGroup("")
    .AddEndpointFilterFactory(ValidationFilter.ValidationEndpointFilterFactory);

all.MapModules<IApiMarker>();

all.AddAuthenticationEndpoints();

all.AddResultEndpoints();

all.AddValidationEndpoints();

app.Run();