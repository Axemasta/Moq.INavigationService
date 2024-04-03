using Moq.Tests.Samples;
namespace Moq.Tests;

public class VerifyUriViewModelTests : FixtureBase<SampleUriViewModel>
{
	#region Setup

	private readonly MockNavigationService navigationService;

	public VerifyUriViewModelTests()
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
