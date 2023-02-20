using Api.Tests;
using Api.Tests.Features.Authentication;
using Api.Tests.Features.Validation;
using FluentValidation;
using Peter.MinimalApi.Modules;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IValidator<Product>, Product.ProductValidator>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapModules<IApiMarker>();

app.AddAuthenticationEndpoints();

app.AddResultEndpoints();

app.AddValidationEndpoints();

app.Run();