using Moq.Tests.Samples.Pages;
namespace Moq.Tests.Samples;

public class SomeBuilderViewModel
{
    private readonly INavigationService navigationService;

    public SomeBuilderViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public async Task<INavigationResult> NavigateToHomePage()
    {
        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToDeepLinkedPage()
    {
        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddSegment<HelloPage>()
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToNavigationHomePage()
    {
        return await navigationService.CreateBuilder()
            .AddNavigationPage()
            .AddSegment<HomePage>()
            .NavigateAsync();
    }

    public async Task NavigateToModalHomePageViaSegment()
    {
        await navigationService.CreateBuilder()
            .AddSegment<HomePage>(true)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToModalHomePageViaParameter()
    {
        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToHomePageWithParameters()
    {
        var navParams = new NavigationParameters
        {
            {
                "KeyOne", "Hello World"
            },
        };

        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .WithParameters(navParams)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToHomePageWithAddParameters()
    {
        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter("KeyOne", "Hello World")
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToHomePageWithMutlipleAddParameters()
    {
        return await navigationService.CreateBuilder()
            .AddSegment<HomePage>()
            .AddParameter("KeyOne", "Hello World")
            .AddParameter("KeyTwo", 123456)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToModalFromParameterNavigationHome()
    {
        return await navigationService.CreateBuilder()
            .AddNavigationPage()
            .AddSegment<HomePage>()
            .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToModalNavigationHome_FromSegmentWithAddSegmentUseModal()
    {
        return await navigationService.CreateBuilder()
            .AddNavigationPage()
            .AddSegment<HomePage>(true)
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToModalNavigationHome_FromSegmentWithAddNavigationUseModal()
    {
        return await navigationService.CreateBuilder()
            .AddNavigationPage(true)
            .AddSegment<HomePage>()
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithHomePageTab()
    {
        return await navigationService.CreateBuilder()
            .AddTabbedSegment(tabbed =>
                tabbed.CreateTab<HomePage>())
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithManyTabs()
    {
        return await navigationService.CreateBuilder()
            .AddTabbedSegment(tabbed =>
                tabbed.CreateTab<HomePage>().CreateTab<HelloPage>())
            .NavigateAsync();
    }

    public async Task<INavigationResult> NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        return await navigationService.CreateBuilder()
            .AddTabbedSegment(tabbed =>
                tabbed.CreateTab(b => b.AddNavigationPage().AddSegment<HomePage>())
                    .CreateTab(b => b.AddNavigationPage().AddSegment<HelloPage>()))
            .NavigateAsync();
    }
}

public class SomeBuilderViewModelTests : FixtureBase<SomeBuilderViewModel>
{
    #region Setup

    private readonly MockNavigationService navigationService;

    public SomeBuilderViewModelTests()
    {
        navigationService = new MockNavigationService();

        navigationService.SetupAllNavigationReturns(true);
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
    public async Task Verify_NavigateToNavigationHomePage()
    {
        // Arrange

        // Act
        await Sut.NavigateToNavigationHomePage();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddNavigationPage()
                .AddSegment<HomePage>()
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
                .WithParameters(new NavigationParameters
                {
                    {
                        "KeyOne", "Hello World"
                    },
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
    public async Task Verify_NavigateToModalHomePageViaSegment()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalHomePageViaSegment();

        // Assert
        navigationService.VerifyNavigation(
            nav => navigationService.CreateBuilder()
                .AddSegment<HomePage>(true)
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalHomePageViaParameter()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalHomePageViaParameter();

        // Assert
        navigationService.VerifyNavigation(
            nav => navigationService.CreateBuilder()
                .AddSegment<HomePage>()
                .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalFromParameterNavigationHomePage()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalFromParameterNavigationHome();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddNavigationPage()
                .AddSegment<HomePage>()
                .AddParameter(KnownNavigationParameters.UseModalNavigation, true)
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalNavigationHome_FromSegmentWithAddSegmentUseModal()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalNavigationHome_FromSegmentWithAddSegmentUseModal();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddNavigationPage()
                .AddSegment<HomePage>(true)
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToModalNavigationHome_FromSegmentWithAddNavigationUseModal()
    {
        // Arrange

        // Act
        await Sut.NavigateToModalNavigationHome_FromSegmentWithAddNavigationUseModal();

        // Assert
        navigationService.VerifyNavigation(
            nav => nav.CreateBuilder()
                .AddNavigationPage(true)
                .AddSegment<HomePage>()
                .NavigateAsync(),
            Times.Once());
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithHomePageTab()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithHomePageTab();

        // Assert
        var ex = Record.Exception(() =>
        {
            navigationService.VerifyNavigation(
                nav => nav.CreateBuilder()
                    .AddTabbedSegment(tabbed =>
                        tabbed.CreateTab<HomePage>())
                    .NavigateAsync(),
                Times.Once());
        });

        Assert.IsType<NotSupportedException>(ex);
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithManyTabs()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithManyTabs();

        // Assert
        var ex = Record.Exception(() =>
        {
            navigationService.VerifyNavigation(
                nav => nav.CreateBuilder()
                    .AddTabbedSegment(tabbed =>
                        tabbed.CreateTab<HomePage>().CreateTab<HelloPage>())
                    .NavigateAsync(),
                Times.Once());
        });

        Assert.IsType<NotSupportedException>(ex);
    }

    [Fact]
    public async Task Verify_NavigateToTabbedPageWithManyTabsAndNavigationPages()
    {
        // Arrange

        // Act
        await Sut.NavigateToTabbedPageWithManyTabsAndNavigationPages();

        // Assert
        var ex = Record.Exception(() =>
        {
            navigationService.VerifyNavigation(
                nav => nav.CreateBuilder()
                    .AddTabbedSegment(tabbed =>
                        tabbed.CreateTab(b => b.AddNavigationPage().AddSegment<HomePage>())
                            .CreateTab(b => b.AddNavigationPage().AddSegment<HelloPage>()))
                    .NavigateAsync(),
                Times.Once());
        });

        Assert.IsType<NotSupportedException>(ex);
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
                nav => nav.CreateBuilder()
                    .AddSegment("ThisIsntTheHelloPage", false)
                    .NavigateAsync(),
                Times.Once());
        });

        // Assert
        Assert.IsType<VerifyNavigationException>(ex);
    }

    #endregion Tests
}
