using Moq.Tests.Samples;

namespace Moq.Tests;

public class SetupUriViewModelTests : FixtureBase<SampleUriViewModel>
{
    #region Setup

    private readonly MockNavigationService navigationService;

    public SetupUriViewModelTests()
    {
        navigationService = new MockNavigationService();
    }

    public override SampleUriViewModel CreateSystemUnderTest()
    {
        return new SampleUriViewModel(navigationService);
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
