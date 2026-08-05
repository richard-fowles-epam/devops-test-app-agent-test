using System.Net.Http.Json;
using CustomerManagement.Api.Models;
using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class GetCustomerSteps
{
    private readonly ScenarioWorld __world;
    private readonly ScenarioState __state;

    public GetCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        __world = world;
        __state = state;
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

        var response = await __world.Client.PostAsJsonAsync("/customers", request);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(created);
        __world.CreatedCustomer = created;
    }

    [When(@"the customer is retrieved by id")]
    public async Task WhenTheCustomerIsRetrievedById()
    {
        Assert.NotNull(__world.CreatedCustomer);
        __state.Response = await __world.Client.GetAsync($"/customers/{__world.CreatedCustomer!.Id}");
    }

    [When(@"a non-existent customer id is requested")]
    public async Task WhenANonExistentCustomerIdIsRequested()
    {
        __state.Response = await __world.Client.GetAsync("/customers/99999");
    }

    [Then(@"the retrieved customer should match the created customer")]
    public async Task ThenTheRetrievedCustomerShouldMatchTheCreatedCustomer()
    {
        Assert.NotNull(__state.Response);

        if (__world.RetrievedCustomer is null)
        {
            var retrieved = await __state.Response!.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(retrieved);
            __world.RetrievedCustomer = retrieved;
        }

        Assert.Equal(__world.CreatedCustomer!.Id, __world.RetrievedCustomer!.Id);
        Assert.Equal(__world.CreatedCustomer.FirstName, __world.RetrievedCustomer.FirstName);
        Assert.Equal(__world.CreatedCustomer.LastName, __world.RetrievedCustomer.LastName);
        Assert.Equal(__world.CreatedCustomer.Email, __world.RetrievedCustomer.Email);
    }
}
