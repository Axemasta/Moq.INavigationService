namespace Moq.Tests.Samples;

/*
 * Test viewmodel using the uri based navigation api
 */

public class SampleUriViewModel
{
    private readonly INavigationService navigationService;

    public SampleUriViewModel(INavigationService navigationService)
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

    public async Task<INavigationResult> NavigateToHomePageWithParametersViaUri()
    {
        var uri = new Uri("HomePage", UriKind.Relative);

        var navParams = new NavigationParameters
        {
            {
                "KeyOne", "Hello World"
            },
        };

        return await navigationService.NavigateAsync(uri, navParams);
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
