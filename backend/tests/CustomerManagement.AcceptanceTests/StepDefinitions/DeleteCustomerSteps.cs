using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class DeleteCustomerSteps
{
    private readonly ScenarioWorld __world;
    private readonly ScenarioState __state;

    public DeleteCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        __world = world;
        __state = state;
    }

    [When(@"the customer is deleted by id")]
    public async Task WhenTheCustomerIsDeletedById()
    {
        Assert.NotNull(__world.CreatedCustomer);
        __state.Response = await __world.Client.DeleteAsync($"/customers/{__world.CreatedCustomer!.Id}");
    }

    [When(@"a non-existent customer id is deleted")]
    public async Task WhenANonExistentCustomerIdIsDeleted()
    {
        __state.Response = await __world.Client.DeleteAsync("/customers/99999");
    }

    [When(@"the deleted customer is retrieved by id")]
    public async Task WhenTheDeletedCustomerIsRetrievedById()
    {
        Assert.NotNull(__world.CreatedCustomer);
        __state.Response = await __world.Client.GetAsync($"/customers/{__world.CreatedCustomer!.Id}");
    }
}
