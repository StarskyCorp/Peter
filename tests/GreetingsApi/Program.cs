using MediatR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddMediatR(typeof(Program));

WebApplication app = builder.Build();

app.MapGet("/{who}", (string who) => $"Hello {who}!");
app.MapGet("/{who}/Authenticated", (string who, HttpContext context) => $"Hello {who}! Here are your claims: {string.Join(",", context.User.Claims)}")
    .RequireAuthorization();

app.MapGetMediatR<Ping, string>("/mediatr/ping");

app.Run();

public partial class Program { } //Test concession

public class Ping : IRequest<string>
{
    public string Name { get; set; }
}

public class PingHandler : IRequestHandler<Ping, string>
{
    public Task<string> Handle(Ping request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Hi {request.Name}! Pong");
    }
}