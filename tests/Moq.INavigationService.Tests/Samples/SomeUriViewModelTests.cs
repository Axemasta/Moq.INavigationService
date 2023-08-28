namespace Moq.Tests.Samples;

public class SomeUriViewModel
{
    private readonly INavigationService navigationService;

    public SomeUriViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public async Task<INavigationResult> NavigateToHomePage()
    {
        return await navigationService.NavigateAsync("HomePage");
    }

    public async Task<INavigationResult> NavigateToHomePageViaUri()
    {
        var uri = new Uri("HomePage", UriKind.Relative);

        return await navigationService.NavigateAsync(uri);
    }

    public async Task<INavigationResult> NavigateToHomePageWithParameters()
    {
        var navParams = new NavigationParameters
        {
            {
                "KeyOne", "Hello World"
            },
        };

        return await navigationService.NavigateAsync("HomePage", navParams);
    }

    public async Task<INavigationResult> NavigateToNavigationHomePage()
    {
        return await navigationService.NavigateAsync("NavigationPage/HomePage");
    }

    public async Task<INavigationResult> NavigateToModalHomePage()
    {
        return await navigationService.NavigateAsync("HomePage", new NavigationParameters
        {
            {
                KnownNavigationParameters.UseModalNavigation, true
            },
        });
    }

    public async Task<INavigationResult> NavigateToModalNavigationHomePage()
    {
        return await navigationService.NavigateAsync("NavigationPage/HomePage", new NavigationParameters
        {
            {
                KnownNavigationParameters.UseModalNavigation, true
            },
        });
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithHomePageTab()
    {
        return await navigationService.NavigateAsync("TabbedPage?createTab=HomePage");
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithManyTabs()
    {
        return await navigationService.NavigateAsync("TabbedPage?createTab=HomePage&createTab=HelloPage");
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        return await navigationService.NavigateAsync("TabbedPage?createTab=NavigationPage|HomePage&createTab=NavigationPage|HelloPage");
    }
}

public class SomeUriViewModelTests : FixtureBase<SomeUriViewModel>
{
    #region Setup

    private readonly MockNavigationService navigationService;

    public SomeUriViewModelTests()
    {
        navigationService = new MockNavigationService();
    }

    public override SomeUriViewModel CreateSystemUnderTest()
    {
        return new SomeUriViewModel(navigationService);
    }

    #endregion Setup

    #region Tests

    [Fact]
    public async Task Verify_NavigateToHomePage()
    {
        // Arrange

        // Act
        await Sut.NavigateToHomePage();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("HomePage"),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToHomePageViaUri()
    {
        // Arrange

        // Act
        await Sut.NavigateToHomePageViaUri();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync(new Uri("HomePage", UriKind.Relative)),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToHomePageWithParameters()
    {
        // Arrange

        // Act
        await Sut.NavigateToHomePageWithParameters();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("HomePage", new NavigationParameters
            {
                {
                    "KeyOne", "Hello World"
                },
            }),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalHomePage()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalNavigationHomePage();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("NavigationPage/HomePage", new NavigationParameters
            {
                {
                    KnownNavigationParameters.UseModalNavigation, true
                },
            }),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalNavigationHomePage()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalNavigationHomePage();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("NavigationPage/HomePage", new NavigationParameters
            {
                {
                    KnownNavigationParameters.UseModalNavigation, true
                },
            }), Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithHomePageTab()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithHomePageTab();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("TabbedPage?createTab=HomePage"),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithManyTabs()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithManyTabs();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("TabbedPage?createTab=HomePage&createTab=HelloPage"),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithManyTabsAndNavigationPages();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.NavigateAsync("TabbedPage?createTab=NavigationPage|HomePage&createTab=NavigationPage|HelloPage"),
            Times.Once());
    }

    [Fact]
    public async Task Verify_WhenVerificationDoesntMatchSetup_ShouldThrow()
    {
        // Arrange
        await Sut.NavigateToHomePage();

        // Act
        var ex = Record.Exception(() =>
        {
            navigationService.VerifyNavigation(
                nav => nav.NavigateAsync("NotTheHomePage"),
                Times.Once());
        });

        // Assert
        Assert.IsType<VerifyNavigationException>(ex);
    }

    #endregion Tests
}
