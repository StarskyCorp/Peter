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

You can take a look at the [validation tests](tests/Api.Tests/Validation) to understand how to use each of them.

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

You will have this other:

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
`Result<T>` is a abstract generic type representing an operation's success or failure. It has no relation with ASP.NET Core and can be used independently. You'll often use it when you need to return not just a value, but also a typed representation of an operation.

The main purpose of `Result<T>` and all their descendants is to avoid the [exception-driven control flow anti-pattern](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control).

Here, you can see a code snippet that doesn't use exceptions to guide control flow and use pattern matching to explore return value type:

```csharp
namespace Peter.Result;

public class CreateOrderHandler
{
    public void Handle(int customerId)
    {
        var orderService = new OrderService();
        var result = orderService.CreateOrder(customerId);
        if (result) // if (result is OkResult<CreateOrderResponse>)
        {
            // result.Value.OrderId
            // result.Value.CustomerId
            // result.Value.CreatedDate
        }
        // else
        // {
        //     throw new Exception("Order was not able to be created");
        // }
        else
            switch (result)
            {
                case NotFoundResult<CreateOrderResponse> notFoundResult:
                    break;
                case InvalidResult<CreateOrderResponse> invalidResult:
                    break;
                case CustomerHasNoCredit customerHasNoCredit:
                    // customerHasNoCredit.CurrentCredit
                    // customerHasNoCredit.AttemptedCredit
                    break;
                case ErrorResult<CreateOrderResponse> errorResult:
                    break;
            }
    }
}

public class OrderService
{
    public Result<CreateOrderResponse> CreateOrder(int customerId)
    {
        var customerByIdResult = GetCustomerById(customerId);
        if (!customerByIdResult)
        {
            // Instead of throw new NotFoundException() or whatever you want...
            return NotFoundResult<CreateOrderResponse>.Create();
        }

        var customer = customerByIdResult.Value!;
        if (!customer.IsGold)
        {
            return InvalidResult<CreateOrderResponse>.Create(nameof(customer.IsGold),
                "Customer must be gold for creating an order");
        }

        if (!HasCredit(customer))
        {
            // Instead of throw new CustomerHasNoCreditException() or whatever you want...
            return CustomerHasNoCredit.Create(currentCredit: 1000, attemptedCredit: 2000);
        }

        try
        {
            // A business rule without a custom type to represent it
        }
        catch (Exception)
        {
            return ErrorResult<CreateOrderResponse>.Create("Something happened");
        }

        // Here goes the remaining code for creating an order, and finally...

        return OkResult<CreateOrderResponse>.Create(new CreateOrderResponse(5, customer.Id, DateTime.UtcNow));
    }

    private Result<Customer> GetCustomerById(int customerId)
    {
        // You should return one of the following...
        // return OkResult<Customer>.Create(new Customer() { Id = customerId});
        return NotFoundResult<Customer>.Create();
    }

    private bool HasCredit(Customer customer)
    {
        return false;
    }
}

public class Customer
{
    public int Id { get; set; }
    public bool IsGold { get; set; }
}

public class CreateOrderResponse
{
    public int OrderId { get; }
    public int CustomerId { get; }
    public DateTime CreatedDate { get; }

    public CreateOrderResponse(int orderId, int customerId, DateTime createdDate)
    {
        OrderId = orderId;
        CustomerId = customerId;
        CreatedDate = createdDate;
    }
}

public class CustomerHasNoCredit : Result<CreateOrderResponse>
{
    public int CurrentCredit { get; }
    public int AttemptedCredit { get; }

    private CustomerHasNoCredit(int currentCredit, int attemptedCredit) : base(false, null)
    {
        CurrentCredit = currentCredit;
        AttemptedCredit = attemptedCredit;
    }

    public static CustomerHasNoCredit Create(int currentCredit, int attemptedCredit)
    {
        return new CustomerHasNoCredit(currentCredit, attemptedCredit);
    }
}
```

*This package has not any relevant dependency.*

### Peter.Result.MinimalApi

With the help of the `ToMinimalApi()` extension method, you can return any of the previous types from your command or query and have a thin controller with minimal code required.

For example, instead of writing this:

```csharp
app.MapGet("customers/{id:int}", async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
    {
        var customer =  await mediator.Send(request);
        if (customer is not null)
        {
            return Results.NotFound();
        }

        return Results.Ok(customer);
    });
```

You could write this other:

```csharp
app.MapGet("customers/{id:int}", async (IMediator mediator, [AsParameters] GetCustomerRequest request) =>
        (await mediator.Send(request)).ToMinimalApi());
```

From the application logic side (and returning the base type `Result<T>`), we return one specialized type or another easily.

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
        return NotFoundResult<GetCustomerResponse>.Create();
    }

    return OkResult<GetCustomerResponse>.Create(customer);
}
```

#### HTTP status codes

The following table shows which HTTP status codes the result types are mapped to and what options we have to configure them.

| Type                | HTTP status code | Behavior                               | Value                                                                   |
| ------------------- | ---------------- | -------------------------------------- |-------------------------------------------------------------------------|
| `OkResult<T>`       | 200 (default)    | `UseOk`                                | Body                                                                    |
|                     | 201              | `UseCreated`<br/>`UseCreatedAtRoute`   | Body<br/>Location header                                                |
|                     | 202              | `UseAccepted`<br/>`UseAcceptedAtRoute` | Body<br/>Location header                                                |
| `ErrorResult<T>`    | 500 (defaul)     | `UseProblem`                           | [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0)                                        |
|                     | 500              | `UseInternalServerError`               | None                                                                    |
| `NotFoundResult<T>` | 404 (default)    | `UseNotFound`                          | Body                                                                    |
|                     | 204              | `UseNoContent`                         | None                                                                    |
| `InvalidResult<T>`  | 400 (default)    | `UseValidationProblem`                 | [HttpValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpvalidationproblemdetails?view=aspnetcore-7.0) collection |
|                     | 400              | `UseBadRequest`                        | [ValidationError](src/Peter.Result/ValidationError.cs) collection       |

*This package has a dependency of [Peter.Result](#peterresult) package.*

#### Extending

##### Inheritance

You can inherit from an existing type:

```csharp
/// <summary>
/// A specialized version of <see cref="OkResult"/>
/// </summary>
public class VeryOkResult<T> : OkResult<T>
{
    protected VeryOkResult(T? value): base(value)
    {
        
    }
    
    public new static VeryOkResult<T> Create(T? value = default)
    {
        return new VeryOkResult<T>(value);
    }
    
    public static implicit operator VeryOkResult<T>(T value) => new(value);
}
```

And then use it in a request endpoint handler. Since it is a type that inherits from `OkResult<T>`, it will behave in the same way:

```csharp
app.MapGet("/very_ok", () =>
{
    var result = VeryOkResult<string>.Create("Peter");
    return result.ToMinimalApi();
});
```

#### Configuration

If you want to customize the behavior of `ToMinimalApi` globally, you'll need to use the `ConfigurePeterMinimalApi` method.

##### Default behavior

Although [HTTP status codes](#http-status-codes) shows the default values, you can change them globally as follows:

```csharp
app.ConfigureToMinimalApi(options =>
{
    options.UseBadRequest();
});
```

##### Custom types

This section covers the need to customize the output of a custom return type.

For example, the custom [TeapotResult](tests/Api/TeapotResult.cs) return type should return a [418](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/418) HTTP result status code.

> `TeapotResult` could have been inherited from `OkResult<T>`, but inheriting from `Result<T>` could be a success or failure.

```csharp
using Peter.Result;

namespace Api;

public class TeapotResult<T> : Result<T>
{
    protected TeapotResult(bool ok, T? value) : base(ok, value)
    {
    }

    public static TeapotResult<T> Create(bool ok, T? value = default)
    {
        return new TeapotResult<T>(ok, value);
    }
}
```

Now, we can configure how to manage `TeapotResult<string>`:

```csharp
app.ConfigureToMinimalApi(options =>
{
    ToMinimalApiOptions.UseCustomHandler(typeof(TeapotResult<string>), result =>
    {
        var teapotResult = (TeapotResult<string>)result;
        return Results.Content($"I'm {(!teapotResult.Ok ? "not " : "")}{teapotResult.Value}'s teapot",
            statusCode: 418);
    });
});
```

And finally, you can use your new custom type seamlessly with Peter.

```csharp
app.MapGet("/teapot", (bool ok) =>
    TeapotResult<string>.Create(ok, "Peter").ToMinimalApi());
```

### Peter.Testing

This packages includes utilities to ease minimal APIs testing.

- `AzuriteFixture`. Using [Testcontainers](https://github.com/testcontainers/testcontainers-dotnet) you will be able to write integration testing that has a dependency of Azure storage services.
- `XUnitLogger`. Because, maybe, you will want to see your generated log output in the test runner.
- `AuthenticateApiFixture`. A fixture ready to encapsulate the complexity of having to authenticate the user during testing.

In the [Api.Tests](tests/Api.Tests) project you can see how to use them in a real testing scenario.

*This package has not any relevant dependency.*

## Acknowledgements

Some of the Peter "tools" are inspired by awesome open-source projects or excellent posts:

- [Acheve Test Host by the xabaril team](https://github.com/Xabaril/Acheve.TestHost)
- [Carter, created by some NancyFx mantainers](https://github.com/CarterCommunity/Carter)
- [Result, created by Steve "Ardalis" Smith](https://github.com/ardalis/Result)
- https://khalidabuhakmeh.com/minimal-api-validation-with-fluentvalidation
- https://benfoster.io/blog/minimal-api-validation-endpoint-filters/.