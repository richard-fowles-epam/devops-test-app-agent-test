using System.Net;
using System.Net.Http.Json;
using CustomerManagement.Api.Models;

namespace CustomerManagement.UnitTests;

public class UpdateCustomerTests
{
    [Fact]
    public async Task PutCustomer_WithValidRequest_UpdatesCustomerAndReturnsOk()
    {
        // Arrange: create a customer first so we have a valid id to update.
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var createRequest = new AddCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };

        var postResponse = await client.PostAsJsonAsync("/customers", createRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);

        var updateRequest = new UpdateCustomerRequest
        {
            FirstName = "Ada Updated",
            LastName = "Lovelace Updated",
            Email = "ada.updated@example.com"
        };

        // Act
        var putResponse = await client.PutAsJsonAsync($"/customers/{created!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var updated = await putResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated!.Id);
        Assert.Equal("Ada Updated", updated.FirstName);
        Assert.Equal("Lovelace Updated", updated.LastName);
        Assert.Equal("ada.updated@example.com", updated.Email);
    }

    [Fact]
    public async Task PutCustomer_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var updateRequest = new UpdateCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };

        // Act
        var response = await client.PutAsJsonAsync("/customers/99999", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PutCustomer_WithMissingRequiredField_ReturnsBadRequest()
    {
        // Arrange: create a customer first so we have a valid id.
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var createRequest = new AddCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };

        var postResponse = await client.PostAsJsonAsync("/customers", createRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);

        // FirstName is empty — should fail validation.
        var updateRequest = new UpdateCustomerRequest
        {
            FirstName = "",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };

        // Act
        var response = await client.PutAsJsonAsync($"/customers/{created!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutCustomer_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange: create a customer first so we have a valid id.
        await using var factory = new CustomerApiFactory();
        var client = factory.CreateClient();

        var createRequest = new AddCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };

        var postResponse = await client.PostAsJsonAsync("/customers", createRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);

        var updateRequest = new UpdateCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "not-an-email"
        };

        // Act
        var response = await client.PutAsJsonAsync($"/customers/{created!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
