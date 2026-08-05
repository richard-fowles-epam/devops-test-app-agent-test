using System.Net.Http.Json;
using CustomerManagement.Api.Models;
using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class GetCustomerSteps
{
    private readonly ScenarioWorld _world;
    private readonly ScenarioState _state;

    public GetCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        _world = world;
        _state = state;
    }

    [Given(@"a customer has already been created")]
    public async Task GivenACustomerHasAlreadyBeenCreated()
    {
        var request = new AddCustomerRequest
        {
            FirstName = "Grace",
            LastName = "Hopper",
            Email = "grace@example.com"
        };

        var response = await _world.Client.PostAsJsonAsync("/customers", request);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);
        _world.CreatedCustomer = created;
    }

    [When(@"the customer is retrieved by id")]
    public async Task WhenTheCustomerIsRetrievedById()
    {
        Assert.NotNull(_world.CreatedCustomer);
        _state.Response = await _world.Client.GetAsync($"/customers/{_world.CreatedCustomer!.Id}");
    }

    [When(@"a non-existent customer id is requested")]
    public async Task WhenANonExistentCustomerIdIsRequested()
    {
        _state.Response = await _world.Client.GetAsync("/customers/99999");
    }

    [Then(@"the retrieved customer should match the created customer")]
    public async Task ThenTheRetrievedCustomerShouldMatchTheCreatedCustomer()
    {
        Assert.NotNull(_state.Response);

        if (_world.RetrievedCustomer is null)
        {
            var retrieved = await _state.Response!.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(retrieved);
            _world.RetrievedCustomer = retrieved;
        }

        Assert.Equal(_world.CreatedCustomer!.Id, _world.RetrievedCustomer!.Id);
        Assert.Equal(_world.CreatedCustomer.FirstName, _world.RetrievedCustomer.FirstName);
        Assert.Equal(_world.CreatedCustomer.LastName, _world.RetrievedCustomer.LastName);
        Assert.Equal(_world.CreatedCustomer.Email, _world.RetrievedCustomer.Email);
    }
}
