using System.Net;
using System.Net.Http.Json;
using CustomerManagement.Api.Models;

namespace CustomerManagement.UnitTests;

public class DeleteCustomerTests
{
    [Fact]
    public async Task DeleteCustomer_WithExistingId_ReturnsNoContent_AndCustomerIsDeleted()
    {
        // Arrange: create a customer first so we have a valid id to delete.
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var request = new AddCustomerRequest
        {
            FirstName = "Alan",
            LastName = "Turing",
            Email = "alan@example.com"
        };

        var postResponse = await client.PostAsJsonAsync("/customers", request);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);

        // Act
        var deleteResponse = await client.DeleteAsync($"/customers/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync($"/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteCustomer_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/customers/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
