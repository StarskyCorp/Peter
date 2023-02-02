WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

app.MapGet("/{who}", (string who) => $"Hello {who}!");
app.MapGet("/{who}/Authenticated", (string who, HttpContext context) => $"Hello {who}! Here are your claims: {string.Join(",", context.User.Claims)}")
    .RequireAuthorization();

app.Run();

public partial class Program { } //Test concession