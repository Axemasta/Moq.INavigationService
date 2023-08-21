namespace Moq.Tests.Samples;

public class SomeUriViewModel
{
	private readonly INavigationService navigationService;

	public SomeUriViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
	}

	public async Task NavigateToHomePage()
	{
		await navigationService.NavigateAsync("HomePage");
	}

    public async Task NavigateToHomePageViaUri()
    {
        var uri = new Uri("HomePage", UriKind.Relative);

        await navigationService.NavigateAsync(uri);
    }

    public async Task NavigateToHomePageWithParameters()
    {
        var navParams = new NavigationParameters()
        {
            { "KeyOne", "Hello World" },
        };

        await navigationService.NavigateAsync("HomePage", navParams);
    }

    public async Task NavigateToNavigationHomePage()
    {
        await navigationService.NavigateAsync("NavigationPage/HomePage");
    }

    public async Task NavigateToModalHomePage()
    {
        await navigationService.NavigateAsync("HomePage", new NavigationParameters()
        {
            { KnownNavigationParameters.UseModalNavigation, true },
        });
    }

    public async Task NavigateToModalNavigationHomePage()
    {
        await navigationService.NavigateAsync("NavigationPage/HomePage", new NavigationParameters()
        {
            { KnownNavigationParameters.UseModalNavigation, true },
        });
    }

    public async Task NavigateToTabbedPageWithHomePageTab()
    {
        await navigationService.NavigateAsync("TabbedPage?createTab=HomePage");
    }

    public async Task NavigateToTabbedPageWithManyTabs()
    {
        await navigationService.NavigateAsync("TabbedPage?createTab=HomePage&createTab=HelloPage");
    }

    public async Task NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        await navigationService.NavigateAsync("TabbedPage?createTab=NavigationPage|HomePage&createTab=NavigationPage|HelloPage");
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
            nav => nav.NavigateAsync("HomePage", new NavigationParameters()
            {
                { "KeyOne", "Hello World" },
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
            nav => nav.NavigateAsync("NavigationPage/HomePage", new NavigationParameters()
            {
                { KnownNavigationParameters.UseModalNavigation, true },
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
            nav => nav.NavigateAsync("NavigationPage/HomePage", new NavigationParameters()
            {
                { KnownNavigationParameters.UseModalNavigation, true },
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

    #endregion Tests
}