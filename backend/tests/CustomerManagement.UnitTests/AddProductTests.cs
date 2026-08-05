using System.Net;
using System.Net.Http.Json;
using CustomerManagement.Api.Models;

namespace CustomerManagement.UnitTests;

public class AddProductTests
{
    [Fact]
    public async Task PostProducts_WithValidRequest_CreatesProductAndReturnsId()
    {
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddProductRequest
        {
            Name = "Laptop",
            Description = "Developer laptop",
            Price = 1299.99m
        };

        var response = await client.PostAsJsonAsync("/products", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(created);
        Assert.True(created!.Id > 0);
        Assert.Equal("Laptop", created.Name);
        Assert.Equal("Developer laptop", created.Description);
        Assert.Equal(1299.99m, created.Price);
    }

    [Fact]
    public async Task PostProducts_WithoutName_ReturnsBadRequestValidationProblem()
    {
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddProductRequest
        {
            Name = string.Empty,
            Description = "No name",
            Price = 10m
        };

        var response = await client.PostAsJsonAsync("/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostProducts_WithoutPrice_ReturnsBadRequestValidationProblem()
    {
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddProductRequest
        {
            Name = "Free item",
            Description = "missing price"
        };

        var response = await client.PostAsJsonAsync("/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostProducts_WithPriceLessThanOrEqualToZero_ReturnsBadRequestValidationProblem()
    {
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddProductRequest
        {
            Name = "Invalid",
            Description = "invalid price",
            Price = 0m
        };

        var response = await client.PostAsJsonAsync("/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
