using CustomerManagement.AcceptanceTests.Support;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagement.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class CommonSteps
{
    private readonly ScenarioState __state;

    public CommonSteps(ScenarioState state) => __state = state;

    [Then(@"the response status is (\d+)")]
    public void ThenTheResponseStatusIs(int expectedStatus)
    {
        Assert.NotNull(__state.Response);
        Assert.Equal(expectedStatus, (int)__state.Response!.StatusCode);
    }
}
