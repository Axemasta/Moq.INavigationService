using Moq.Tests.Samples;

namespace Moq.Tests;

public class SetupBuilderViewModelTests : FixtureBase<SampleBuilderViewModel>
{
    #region Setup

    private readonly MockNavigationService navigationService;

    public SetupBuilderViewModelTests()
    {
        navigationService = new MockNavigationService();
    }

    public override SampleBuilderViewModel CreateSystemUnderTest()
    {
        return new SampleBuilderViewModel(navigationService);
    }

    #endregion Setup

    #region Tests

    [Fact]
    public async Task Setup_NavigateToHomePage()
    {
        // Arrange
        var expectedNavigationResult = new NavigationResult();

        // Act
        var result = await Sut.NavigateToHomePage();

        // Assert
        Assert.Equal(expectedNavigationResult, result);
    }

    #endregion Tests
}
