using Moq.Tests.Samples.Pages;

namespace Moq.Tests.Samples;

public class SomeBuilderViewModel
{
    private readonly INavigationService navigationService;

    public SomeBuilderViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public async Task NavigateToHomePage()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .NavigateAsync();
    }

    public async Task NavigateToDeepLinkedPage()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddSegment<HelloPage>()
            .NavigateAsync();
    }

    public async Task NavigateToNavigationHomePage()
    {
        await navigationService.CreateBuilder()
            .AddNavigationPage()
            .AddSegment<HomePage>()
            .NavigateAsync();
    }

    public async Task NavigateToModalHomePage()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
            .NavigateAsync();
    }

    public async Task NavigateToHomePageWithParameters()
    {
        var navParams = new NavigationParameters()
        {
            { "KeyOne", "Hello World" },
        };

        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .WithParameters(navParams)
            .NavigateAsync();
    }

    public async Task NavigateToHomePageWithAddParameters()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter("KeyOne", "Hello World")
            .NavigateAsync();
    }

    public async Task NavigateToHomePageWithMutlipleAddParameters()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter("KeyOne", "Hello World")
            .AddParameter("KeyTwo", 123456)
            .NavigateAsync();
    }

    public async Task NavigateToModalNavigationHomePage()
    {
        await navigationService.CreateBuilder()
            .AddNavigationPage()
            .AddSegment<HomePage>()
            .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
            .NavigateAsync();
    }

    public async Task NavigateToTabbedPageWithHomePageTab()
    {
        await navigationService.CreateBuilder()
            .AddTabbedSegment(tabbed =>
                tabbed.CreateTab<HomePage>())
            .NavigateAsync();
    }

    public async Task NavigateToTabbedPageWithManyTabs()
    {
        await navigationService.CreateBuilder()
            .AddTabbedSegment(tabbed =>
                tabbed.CreateTab<HomePage>().CreateTab<HelloPage>())
            .NavigateAsync();
    }

    public async Task NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        throw new NotImplementedException("I need to workout how to make this one!");
        await navigationService.NavigateAsync("TabbedPage?createTab=NavigationPage|HomePage&createTab=NavigationPage|HelloPage");
    }
}


public class SomeBuilderViewModelTests : FixtureBase<SomeBuilderViewModel>
{
    #region Setup

    private readonly MockNavigationService navigationService;

    public SomeBuilderViewModelTests()
    {
        navigationService = new MockNavigationService();
    }

    public override SomeBuilderViewModel CreateSystemUnderTest()
    {
        return new SomeBuilderViewModel(navigationService);
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
            nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToDeepLinkedPage()
    {
        // Arrange

        // Act
        await Sut.NavigateToDeepLinkedPage();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .AddSegment<HelloPage>()
                .NavigateAsync(),
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
            nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .WithParameters(new NavigationParameters()
                {
                    { "KeyOne", "Hello World" },
                })
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToHomePageWithAddParameters()
    {
        // Arrange

        // Act
        await Sut.NavigateToHomePageWithAddParameters();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .AddParameter("KeyOne", "Hello World")
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToHomePageWithMultipleAddParameters()
    {
        // Arrange

        // Act
        await Sut.NavigateToHomePageWithMutlipleAddParameters();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddSegment<HomePage>()
                .AddParameter("KeyOne", "Hello World")
                .AddParameter("KeyTwo", 123456)
                .NavigateAsync(),
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
            nav => nav.NavigateAsync("NavigationPage/HomePage"), // Note sut uses CreateBuilder api, but this is hard to mock here...
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
            nav => nav.NavigateAsync("TabbedPage?createTab=HomePage"), // Note sut uses CreateBuilder api, but this is hard to mock here...
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
            nav => nav.NavigateAsync("TabbedPage?createTab=HomePage&createTab=HelloPage"), // Note sut uses CreateBuilder api, but this is hard to mock here...
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
            nav => nav.NavigateAsync("TabbedPage?createTab=NavigationPage|HomePage&createTab=NavigationPage|HelloPage"), // Note sut uses CreateBuilder api, but this is hard to mock here...
            Times.Once());
    }

    #endregion Tests
}