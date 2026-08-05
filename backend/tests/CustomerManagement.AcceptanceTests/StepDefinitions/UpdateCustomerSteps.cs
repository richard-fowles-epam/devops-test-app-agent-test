using System.Net.Http.Json;
using CustomerManagement.Api.Models;
using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class UpdateCustomerSteps
{
    private readonly ScenarioWorld __world;
    private readonly ScenarioState __state;

    public UpdateCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        __world = world;
        __state = state;
    }

    [When(@"the customer is updated with the following details")]
    public async Task WhenTheCustomerIsUpdatedWithTheFollowingDetails(Table table)
    {
        Assert.NotNull(__world.CreatedCustomer);
        var row = table.Rows[0];
        __world.UpdateRequest = new UpdateCustomerRequest
        {
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            Email = row["Email"]
        };
        __state.Response = await __world.Client.PutAsJsonAsync(
            $"/customers/{__world.CreatedCustomer!.Id}", __world.UpdateRequest);
    }

    [When(@"a non-existent customer id is updated with valid details")]
    public async Task WhenANonExistentCustomerIdIsUpdatedWithValidDetails()
    {
        var request = new UpdateCustomerRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com"
        };
        __state.Response = await __world.Client.PutAsJsonAsync("/customers/99999", request);
    }

    [Then(@"the updated customer should match the submitted details")]
    public async Task ThenTheUpdatedCustomerShouldMatchTheSubmittedDetails()
    {
        Assert.NotNull(__state.Response);

        if (__world.UpdatedCustomer is null)
        {
            var updated = await __state.Response!.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(updated);
            __world.UpdatedCustomer = updated;
        }

        Assert.Equal(__world.UpdateRequest!.FirstName, __world.UpdatedCustomer!.FirstName);
        Assert.Equal(__world.UpdateRequest.LastName, __world.UpdatedCustomer.LastName);
        Assert.Equal(__world.UpdateRequest.Email, __world.UpdatedCustomer.Email);
    }
}
