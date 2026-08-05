using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class DeleteCustomerSteps
{
    private readonly ScenarioWorld _world;
    private readonly ScenarioState _state;

    public DeleteCustomerSteps(ScenarioWorld world, ScenarioState state)
    {
        _world = world;
        _state = state;
    }

    [When(@"the customer is deleted by id")]
    public async Task WhenTheCustomerIsDeletedById()
    {
        Assert.NotNull(_world.CreatedCustomer);
        _state.Response = await _world.Client.DeleteAsync($"/customers/{_world.CreatedCustomer!.Id}");
    }

    [When(@"a non-existent customer id is deleted")]
    public async Task WhenANonExistentCustomerIdIsDeleted()
    {
        _state.Response = await _world.Client.DeleteAsync("/customers/99999");
    }

    [When(@"the deleted customer is retrieved by id")]
    public async Task WhenTheDeletedCustomerIsRetrievedById()
    {
        Assert.NotNull(_world.CreatedCustomer);
        _state.Response = await _world.Client.GetAsync($"/customers/{_world.CreatedCustomer!.Id}");
    }
}
