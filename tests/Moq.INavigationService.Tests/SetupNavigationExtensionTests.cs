namespace Moq.Tests;

public class SetupNavigationExtensionTests
{
    private MockNavigationService navigationService;

    public SetupNavigationExtensionTests()
    {
        navigationService = new MockNavigationService();
    }

    [Fact]
    public async Task SetupAllNavigationReturns_Should_MakeAllNavigationCallsReturnValue()
    {
        // No Setup Should Return Null
        var preSetupResult = await navigationService.NavigateAsync("HomePage");
        Assert.Null(preSetupResult);

        // True setup should return true
        navigationService.SetupAllNavigationReturns(true);
        var trueSetupResult1 = await navigationService.NavigateAsync("HomePage");
        var trueSetupResult2 = await navigationService.NavigateAsync("HelloWorldPage");
        var trueSetupResult3 = await navigationService.NavigateAsync("SomewhereElsePage");

        Assert.True(trueSetupResult1.Success);
        Assert.True(trueSetupResult2.Success);
        Assert.True(trueSetupResult3.Success);

        // False setup should return false
        navigationService.SetupAllNavigationReturns(false);
        var falseSetupResult1 = await navigationService.NavigateAsync("HomePage");
        var falseSetupResult2 = await navigationService.NavigateAsync("HelloWorldPage");
        var falseSetupResult3 = await navigationService.NavigateAsync("SomewhereElsePage");

        Assert.False(falseSetupResult1.Success);
        Assert.False(falseSetupResult2.Success);
        Assert.False(falseSetupResult3.Success);
    }

    [Fact]
    public async Task SetupAllNavigationFails_WhenProvidedException_ShouldReturnFailedNavigationResult()
    {
        // No Setup Should Return Null
        var preSetupResult = await navigationService.NavigateAsync("HomePage");
        Assert.Null(preSetupResult);

        var expectedException = new Exception("Navigation fell over and the app crashed");

        // True setup should return true
        navigationService.SetupAllNavigationFails(expectedException);
        var result1 = await navigationService.NavigateAsync("HomePage");
        var result2 = await navigationService.NavigateAsync("HelloWorldPage");
        var result3 = await navigationService.NavigateAsync("SomewhereElsePage");

        Assert.False(result1.Success);
        Assert.False(result2.Success);
        Assert.False(result3.Success);

        Assert.Equal(expectedException, result1.Exception);
        Assert.Equal(expectedException, result2.Exception);
        Assert.Equal(expectedException, result3.Exception);
    }
}

