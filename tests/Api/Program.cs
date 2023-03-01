using Api;
using Api.Features.Authentication;
using Api.Features.Validation;
using FluentValidation;
using Peter.MinimalApi.Modules;
using Peter.MinimalApi.Validation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();

var app = builder.Build();

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