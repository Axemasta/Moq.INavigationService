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
    public async Task Setup_WhenBuilderExpressionDoesntCallNavigateAsync_ShouldThrow()
    {
        // Arrange
        var expectedNavigationResult = new NavigationResult();

        // Act
        var ex = Record.Exception(() =>
        {
            navigationService.SetupNavigation(nav => nav.CreateBuilder()
                    .AddSegment<HomePage>()
                    .AddNavigationPage()
                    .AddParameter("ThisWill", "Throw"))
                .ReturnsAsync(expectedNavigationResult);
        });

        // Assert
        Assert.IsType<NotSupportedException>(ex);
        Assert.Equal("Calling method named AddParameter is not supported, only NavigateAsync needs to use mock expressions.", ex.Message);
    }

    [Fact]
    public async Task Setup_WhenBuilderExpressionDoesntCallAnyMethod_ShouldThrow()
    {
        // Arrange
        var expectedNavigationResult = new NavigationResult();

        // Act
        var ex = Record.Exception(() =>
        {
            navigationService.SetupNavigation(expression: null!)
                .ReturnsAsync(expectedNavigationResult);
        });

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Could not determine calling method name", ex.Message);
    }

    [Fact]
    public async Task Setup_NavigateToHomePageWhenSetupIsMatched_ShouldReturnResult()
    {
        // Arrange
        var expectedNavigationResult = new NavigationResult();

        navigationService.SetupNavigation(nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .NavigateAsync())
            .ReturnsAsync(expectedNavigationResult);

        // Act
        var result = await Sut.NavigateToHomePage();

        // Assert
        Assert.Equal(expectedNavigationResult, result);
        navigationService.Verify();
    }

    [Fact]
    public async Task Setup_NavigateToHomePageWhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange
        var expectedNavigationResult = new NavigationResult();

        navigationService.SetupNavigation(nav => nav.CreateBuilder()
                .AddNavigationPage()
                .AddSegment<HomePage>()
                .NavigateAsync())
            .ReturnsAsync(expectedNavigationResult);

        // Act
        var result = await Sut.NavigateToHomePage();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task NavigateToHomePageWithParameters_WhenSetupIsMatched_ShouldReturnSetup()
    {
        // Arrange
        var correctParams = new NavigationParameters
        {
            {
                "KeyOne", "Hello World"
            },
        };

        var expectedNavigationResult = new NavigationResult();

        navigationService.SetupNavigation(nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .WithParameters(correctParams)
                .NavigateAsync())
            .ReturnsAsync(expectedNavigationResult);

        // Act
        var result = await Sut.NavigateToHomePageWithParameters();

        // Assert
        Assert.Equal(expectedNavigationResult, result);
        navigationService.Verify();
    }

    [Fact]
    public async Task NavigateToHomePageWithParameters_WhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange
        var incorrectParams = new NavigationParameters
        {
            {
                "KeyTwo", 3219092701313
            },
        };

        var expectedNavigationResult = new NavigationResult();

        navigationService.SetupNavigation(nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .WithParameters(incorrectParams)
                .NavigateAsync())
            .ReturnsAsync(expectedNavigationResult);

        // Act
        var result = await Sut.NavigateToHomePageWithParameters();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task NavigateToHomePageWithAddParameters_WhenSetupIsMatched_ShouldReturnSetup()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToHomePageWithAddParameters_WhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToHomePageWithMultipleAddParameters_WhenSetupIsMatched_ShouldReturnSetup()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToHomePageWithMultipleAddParameters_WhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToModalHomePageViaSegment_WhenSetupIsMatched_ShouldReturnSetup()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToModalHomePageViaSegment_WhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToModalHomePageViaParameter_WhenSetupIsMatched_ShouldReturnSetup()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Fact]
    public async Task NavigateToModalHomePageViaParameter_WhenSetupIsNotMatched_ShouldReturnNull()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    #endregion Tests
}
