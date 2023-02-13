using System.Net;
using System.Net.Http.Json;
using Api.Tests;
using Api.Tests.Features.Commands;
using Api.Tests.Features.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Peter.MinimalApi.MediatR.Tests;

public class MediatRModuleExtensionsShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public MediatRModuleExtensionsShould(WebApplicationFactory<IApiMarker> factory) => _client = factory.CreateDefaultClient();

    [Fact]
    public async Task map_get_to_query()
    {
        var response = await _client.GetFromJsonAsync<IEnumerable<Product>>("/MediatRGroupProducts?name=P1");
        response.Should().HaveCount(1);
        response.First().Name.Should().Be("P1");
    }

    [Fact]
    public async Task map_post_to_command()
    {
        var addProductCommand = new AddProductCommand { Name = "New Product" };
        var response = await _client.PostAsJsonAsync("/MediatRGroupProducts", addProductCommand);
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var addedProduct = JsonConvert.DeserializeObject<Product>(readAsStringAsync);
        addedProduct.Name.Should().Be("New Product");
        addedProduct.Id.Should().Be(99);
        //TODO: Check with System.Text.Json
    }

    [Fact]
    public async Task map_put_to_command()
    {
        var updateProductCommand = new UpdateProductCommand { Id = 22, Name = "Updated Product" };
        var response = await _client.PutAsJsonAsync("/MediatRGroupProducts", updateProductCommand);
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var addedProduct = JsonConvert.DeserializeObject<Product>(readAsStringAsync);
        addedProduct.Name.Should().Be("Updated Product");
        addedProduct.Id.Should().Be(22);
        //TODO: Check with System.Text.Json
    }

    [Fact]
    public async Task map_patch_to_command()
    {
        var updateProductCommand = new UpdateProductCommand { Id = 32, Name = "Updated again" };
        var response = await _client.PatchAsJsonAsync("/MediatRGroupProducts", updateProductCommand);
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var addedProduct = JsonConvert.DeserializeObject<Product>(readAsStringAsync);
        addedProduct.Name.Should().Be("Updated again");
        addedProduct.Id.Should().Be(32);
        //TODO: Check with System.Text.Json
    }

    [Fact]
    public async Task map_delete_to_query()
    {
        var response = await _client.DeleteAsync("/MediatRGroupProducts/5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}