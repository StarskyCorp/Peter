﻿using Api.Tests;
using Api.Tests.Features.Commands;
using Api.Tests.Features.Queries;
using Api.Tests.Features.Validation;
using FluentValidation;
using MediatR;
using Peter.MinimalApi.Modules;
using Peter.Result;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IValidator<Product>, Product.ProductValidator>();
builder.Services.AddMediatR(typeof(IApiMarker));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapModules<IApiMarker>();
app.MapGet("/{who}", (string who) => $"Hello {who}!");
app.MapGet("/{who}/Authenticated", (string who, HttpContext context) =>
{
    return $"Hello {who}! Here are your claims: {string.Join(",", context.User.Claims)}";
}).RequireAuthorization();

app.MapGet("/ok", () =>
{
    Result<string> result = "Peter";
    return result.ToMinimalApi();
});

app.MapGet("/failed", () =>
{
    var result = Result<object>.CreateFailure(new[] { "A failure" });
    return result.ToMinimalApi();
});
app.MapGet("/failed_no_problem_details", () =>
{
    var result = Result<object>.CreateFailure(new[] { "A failure" });
    return result.ToMinimalApi(options =>
    {
        options.UseProblemDetails = false;
    });
});

app.MapGet("/not_exists", () =>
{
    var result = Result<object>.CreateNotExists();
    return result.ToMinimalApi();
});

app.MapGet("/not_exists_with_value", () =>
{
    var result = Result<object>.CreateNotExists("Peter");
    return result.ToMinimalApi();
});

app.MapGet("/invalid", () =>
{
    var result = Result<object>.CreateInvalid(new[] { new ValidationError("peter", "message") });
    return result.ToMinimalApi();
});

app.MapGetMediatR<ProductsQuery, IEnumerable<Product>>("/mediatrProducts");
app.MapPostMediatR<AddProductCommand, Product>("/mediatrProducts");
app.MapPutMediatR<UpdateProductCommand, Product>("/mediatrProducts");
app.MapPatchMediatR<UpdateProductCommand, Product>("/mediatrProducts");

app.AddValidationEndpoints();

app.Run();