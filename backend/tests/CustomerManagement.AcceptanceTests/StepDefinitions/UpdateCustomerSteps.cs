using System.Net.Http.Json;
using CustomerManagement.Api.Models;
using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class UpdateCustomerSteps
{
    private readonly ScenarioWorld _world;
    private readonly ScenarioState _state;

    public UpdateCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        _world = world;
        _state = state;
    }

    [When(@"the customer is updated with the following details")]
    public async Task WhenTheCustomerIsUpdatedWithTheFollowingDetails(Table table)
    {
        Assert.NotNull(_world.CreatedCustomer);
        var row = table.Rows[0];
        _world.UpdateRequest = new UpdateCustomerRequest
        {
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            Email = row["Email"]
        };
        _state.Response = await _world.Client.PutAsJsonAsync(
            $"/customers/{_world.CreatedCustomer!.Id}", _world.UpdateRequest);
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
        _state.Response = await _world.Client.PutAsJsonAsync("/customers/99999", request);
    }

    [Then(@"the updated customer should match the submitted details")]
    public async Task ThenTheUpdatedCustomerShouldMatchTheSubmittedDetails()
    {
        Assert.NotNull(_state.Response);

        if (_world.UpdatedCustomer is null)
        {
            var updated = await _state.Response!.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(updated);
            _world.UpdatedCustomer = updated;
        }

        Assert.Equal(_world.UpdateRequest!.FirstName, _world.UpdatedCustomer!.FirstName);
        Assert.Equal(_world.UpdateRequest.LastName, _world.UpdatedCustomer.LastName);
        Assert.Equal(_world.UpdateRequest.Email, _world.UpdatedCustomer.Email);
    }
}
