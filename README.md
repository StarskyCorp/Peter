# Peter

[![Build status](https://github.com/StarskyCorp/Peter/actions/workflows/ci.yaml/badge.svg?ref=main)](https://github.com/StarskyCorp/Peter/actions?query=workflow%3ACI) 
[![NET](https://img.shields.io/badge/dotnet%20version-net7.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## About Peter

Peter is a set of tools that may help you creating and testing your ASP.NET Core Minimal APIs.

It provides:

- A test server ready to use for integration testing of your API, supporting secured endpoints.
- A generic type `Result<T>` useful to return any kind of specialized result from a command or query, being able to map them to http results.
- A solution for Minimal API modules organization.
- A solution for automatic Minimal API request validation.

## Where does the name "Peter" come from?

There is a spanish expression *"No lo usa ni Peter"* than means "nobody use it!".

As this package is pretty new, we suppose nobody is using it yet. We hope it will change soon. Let us know!

## Packages

### Peter.MinimalApi

This package includes two utilities to solve common problems when you are developing minimal APIs. The first one is automatic validation and the second one is a modular organization of the request handlers into modules.

#### Validation

There are two options inspired by https://khalidabuhakmeh.com/minimal-api-validation-with-fluentvalidation and https://benfoster.io/blog/minimal-api-validation-endpoint-filters/ respectively.

You can take a look at the [validation tests](tests/Peter.Api.Tests/Validation) to understand how to use each of them.

#### Modules

Inspired by [Carter](https://github.com/CarterCommunity/Carter) and https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6 you can organize your endpoints into modules for better organization.

Instead of this:

```csharp
// Program.cs
app.MapPost("customers",
    async (IMediator mediator, CreateCustomerRequest request) =>
        (await mediator.Send(request)).ToMinimalApi());

app.MapGet("customers",
    async (IMediator mediator, [AsParameters] GetCustomersRequest request) =>
        await mediator.Send(request));

app.MapGet("customers/{id:int}",
    async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
        (await mediator.Send(request)).ToMinimalApi());
```

You will have this other in a separated and well organized file:

```csharp
// Program.cs
app.MapModules<IApiMarker>();  // IApiMarker is a marker interface, you can use whatever you want

// Modules/CustomersModule.cs or whatever you want
public class CustomersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("customers");

        group.MapPost("/",
            async (IMediator mediator, CreateCustomerRequest request) =>
                (await mediator.Send(request)).ToMinimalApi());

        group.MapGet("/",
            async (IMediator mediator, [AsParameters] GetCustomersRequest request) =>
                await mediator.Send(request));

        group.MapGet("/{id:int}",
            async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
                (await mediator.Send(request)).ToMinimalApi());

        return app;
    }
}
```

*This package has not any relevant dependency.*

### Peter.Result
`Result<T>` is a generic type representing an operation's success or failure. It has no relation with ASP.NET Core and can be used independently. You'll often use it when you need to return not just a value, but also a typed representation of an operation. Including, optionally, a list of errors in case it has not been satisfactory.

The main purpose of `Result<T>` is to avoid the [exception-driven control flow anti-pattern](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control).

For example, instead of writing this:

```csharp
public (bool success, IEnumerable<string> errors) Validate() 
{
    // Remaining code goes here
}

var (success, errors) = Validate();
if (!success)
{
    // Do something with errors...
}
```

You could write this:

```csharp
public Result<bool> Validate()
{
    // Remaining code goes here
}

var result = Validate();
if (!result)
{
    // Do something with result.Errors...
}
```

Or even this with the help of implicit type conversion:

```csharp
if (!Validate())
{
    // I don't care about mistakes
}
```

A more elaborate example could be this:

```csharp
public class CreateOrderCommand
{
    public void Execute(int customerId)
    {
        var orderService = new OrderService();
        var result = orderService.CreateOrder(customerId);
        if (result)
        {
            // result.Value.CustomerId
            // result.Value.CreatedDate
        }
        else if (result is CustomerNotFoundCreateOrderResult customerNotFoundCreateOrderResult)
        {
            // customerNotFoundCreateOrderResult.CustomerId
        }
    }
}

public class OrderService
{
    public Result<CreateOrderResult> CreateOrder(int customerId)
    {
        Customer? customerId = GetCustomerById(customerId);
        if (customerId is null)
        {
            return new CustomerNotFoundCreateOrderResult(customerId);
        }
        return new CreateOrderResult(customerId, DateTime.UtcNow);
    }
}

public class CreateOrderResult
{
    public int CustomerId { get; }
    public DateTime CreatedDate { get; }

    public void CreateOrderResult(int customerId, DateTime createdDate) 
    {
        CustomerId = customerId;
        CreatedDate = createdDate;
    }
}


public class CustomerNotFoundCreateOrderResult: Result<CreateOrderResult>
{
    public int CustomerId { get; }

    public CustomerNotFoundCreateOrderResult(int customerId): base(false, null)
    {
        CustomerId = customerId;
    }    
}
```

*This package has not any relevant dependency.*

### Peter.Result.MinimalApi

Using `Result<T>`, this package creates new result types, that are tied in some way to ASP.NET Core.

- `NotExistResult<T>`
- `InvalidResult<T>`

With the help of the `ToMinimalApi()` extension method, you can return any of these types (including the `Result<T>` base type) from your command or query and have a thin controller with minimal code required.

For example, instead of writing this:

```csharp
app.MapGet("customers/{id:int}", async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
    {
        var customer =  await mediator.Send(request);
        if (customer is not null)
        {
            return Results.NotExist();
        }

        return Results.Ok(customer);
    });
```

You could write this and the final result would be the same (including both, not found and ok result):

```csharp
app.MapGet("customers/{id:int}", async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
        (await mediator.Send(request)).ToMinimalApi());
```

From the query side (and returning the base type `Result<T>`), we return one specialized type or another easily.

```csharp
public async Task<Result<GetCustomerResponse>> Handle(
    GetCustomerRequest request,
    CancellationToken cancellationToken)
{
    var customer =
        await _context.Customers.FindAsync(new object?[] { request.Id },
            cancellationToken: cancellationToken);
    if (customer is null)
    {
        return NotExistResult<GetCustomerResponse>.Create();
    }

    return Result<GetCustomerResponse>.CreateSuccess(customer);
}
```

The following table shows which HTTP status codes the result types are mapped to and what options we have to configure them.

| Type                                   | HTTP status code                                                                                                                                | Value                                                                                                  |
|----------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------|
| `NotExistResult<T>`                    | 404 (default) `WithNotFoundBehaviour`<br/>204 `WithNoContentBehaviour`                                                                          | In the body payload if 404                                                                             |
| `InvalidResult<T>`                        | 400                                                                                                                                             | [HttpValidationProblemDetails](HttpValidationProblemDetails) collection in the body payload            |
| `Result<T>`<br/>*If `Success` is `false`* | 500                                                                                                                                             | [ProblemDetails](ProblemDetails) if `UseProblemDetails` is `true` (default), otherwise no body payload |
| `Result<T>`<br/>*If `Success` is `true`*  | 200 (default)<br/>201 `WithCreatedBehaviour`<br/>201 `WithCreatedAtBehaviour`<br/>202 `WithAcceptedBehaviour`<br/>202 `WithAcceptedAtBehaviour` | Body payload<br/>Location header if 201 or 202                                                         |

*This package has a dependency of [Peter.Result](#peterresult) package.*

### Peter.Testing

This packages includes utilities to ease minimal APIs testing.

- `AzuriteFixture`. Using [Testcontainers](https://github.com/testcontainers/testcontainers-dotnet) you will be able to write integration testing that has a dependency of Azure storage services.
- `XUnitLogger`. Because, maybe, you will want to see your generated log output in the test runner.
- `AuthenticateApiFixture`. A fixture ready to encapsulate the complexity of having to authenticate the user during testing.

In the [Peter.Api.Tests](tests/Peter.Api.Tests) project you can see how to use them in a real testing scenario.

*This package has not any relevant dependency.*

## Acknowledgements

Peter is built using the some existing open source projects like:

- [xUnit.net](https://xunit.net/)
- [Fluent Assertions](https://fluentassertions.com/)

In addition, some of the Peter "tools" are inspired by awesome open source projects like:

- [Acheve Test Host by the xabaril team](https://github.com/Xabaril/Acheve.TestHost)
- [Carter, created by some NancyFx mantainers](https://github.com/CarterCommunity/Carter)
- [Result, created by Steve "Ardalis" Smith](https://github.com/ardalis/Result)