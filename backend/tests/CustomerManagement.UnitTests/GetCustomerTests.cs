using System.Net;
using System.Net.Http.Json;
using CustomerManagement.Api.Models;

namespace CustomerManagement.UnitTests;

public class GetCustomerTests
{
    [Fact]
    public async Task GetCustomer_WithExistingId_ReturnsOkAndCustomer()
    {
        // Arrange: create a customer first so we have a valid id to look up.
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddCustomerRequest
        {
            FirstName = "Grace",
            LastName = "Hopper",
            Email = "grace@example.com"
        };

        var postResponse = await client.PostAsJsonAsync("/customers", request);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);

        // Act
        var getResponse = await client.GetAsync($"/customers/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrieved = await getResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(retrieved);
        Assert.Equal(created.Id, retrieved!.Id);
        Assert.Equal("Grace", retrieved.FirstName);
        Assert.Equal("Hopper", retrieved.LastName);
        Assert.Equal("grace@example.com", retrieved.Email);
    }

    [Fact]
    public async Task GetCustomer_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/customers/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
