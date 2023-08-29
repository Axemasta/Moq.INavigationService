using Moq.Tests.Samples;

namespace Moq.Tests;

/*
 * Test viewmodel using the uri based navigation api
 */

public class SampleBuilderViewModel
{
    private readonly INavigationService navigationService;

    public SampleBuilderViewModel(INavigationService navigationService)
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

    public async Task<INavigationResult> NavigateToModalHomePageViaSegment()
    {
        return await navigationService.CreateBuilder()
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
