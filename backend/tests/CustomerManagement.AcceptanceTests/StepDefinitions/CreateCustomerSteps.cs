using System.Net.Http.Json;
using CustomerManagement.Api.Models;
using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class CreateCustomerSteps
{
    private readonly ScenarioWorld __world;
    private readonly ScenarioState __state;

    public CreateCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        __world = world;
        __state = state;
    }

    [Given(@"a customer with the following details")]
    public void GivenACustomerWithTheFollowingDetails(Table table)
    {
        var row = table.Rows[0];
        __world.Request = new AddCustomerRequest
        {
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            Email = row["Email"]
        };
    }

    [When(@"the customer is submitted to POST /customers")]
    public async Task WhenTheCustomerIsSubmittedToPostCustomers()
    {
        Assert.NotNull(__world.Request);
        __state.Response = await __world.Client.PostAsJsonAsync("/customers", __world.Request);
    }

    [Then(@"the created customer should match the submitted details")]
    public async Task ThenTheCreatedCustomerShouldMatchTheSubmittedDetails()
    {
        var created = await ReadCreatedCustomerAsync();
        Assert.Equal(__world.Request!.FirstName, created.FirstName);
        Assert.Equal(__world.Request.LastName, created.LastName);
        Assert.Equal(__world.Request.Email, created.Email);
    }

    [Then(@"the created customer should have a generated id")]
    public async Task ThenTheCreatedCustomerShouldHaveAGeneratedId()
    {
        var created = await ReadCreatedCustomerAsync();
        Assert.True(created.Id > 0);
    }

    private async Task<Customer> ReadCreatedCustomerAsync()
    {
        Assert.NotNull(__state.Response);

        // The response content stream can only be read once, so cache the
        // parsed customer for any subsequent steps in the same scenario.
        if (__world.CreatedCustomer is null)
        {
            var created = await __state.Response!.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(created);
            __world.CreatedCustomer = created;
        }

        return __world.CreatedCustomer!;
    }
}
