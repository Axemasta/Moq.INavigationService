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

	[Fact]
	public async Task Verify_GoBackWithNoParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackWithNoParameters();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.GoBackAsync(),
			Times.Once());
	}

	[Fact]
	public async Task Verify_GoBackToWithNoParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackToWithNoParameters();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.GoBackToAsync("DestinationPage"),
			Times.Once());
	}

	[Fact]
	public async Task Verify_GoBackToRootWithNoParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackToRootWithNoParameters();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.GoBackToRootAsync(),
			Times.Once());
	}

	[Fact]
	public async Task Verify_GoBackWithParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackWithParameters();

		// Assert
		var expectedParams = new NavigationParameters() { { "KeyOne", "ValueOne" } };

		navigationService.VerifyNavigation(
			nav => nav.GoBackAsync(expectedParams),
			Times.Once());
	}

	[Fact]
	public async Task Verify_GoBackToWithParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackToWithParameters();

		// Assert
		var expectedParams = new NavigationParameters() { { "KeyOne", "ValueOne" } };

		navigationService.VerifyNavigation(
			nav => nav.GoBackToAsync("DestinationPage", expectedParams),
			Times.Once());
	}

	[Fact]
	public async Task Verify_GoBackToRootWithParameters()
	{
		// Arrange

		// Act
		await Sut.GoBackToRootWithParameters();

		// Assert
		var expectedParams = new NavigationParameters() { { "KeyOne", "ValueOne" } };

		navigationService.VerifyNavigation(
			nav => nav.GoBackToRootAsync(expectedParams),
			Times.Once());
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePage()
	{
		// Arrange

		// Act
		await Sut.NavigateToAbsoluteHomePage();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.NavigateAsync("/NavigationPage/HomePage"),
			Times.Once());
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageViaUri()
	{
		// Arrange

		// Act
		await Sut.NavigateToAbsoluteHomePageViaUri();

		// Assert
		var expectedUri = new Uri("/NavigationPage/HomePage", UriKind.Relative);

		navigationService.VerifyNavigation(
			nav => nav.NavigateAsync(expectedUri),
			Times.Once());
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageWithParameters()
	{
		// Arrange

		// Act
		await Sut.NavigateToAbsoluteHomePageWithParameters();

		// Assert
		var expectedParams = new NavigationParameters()
		{
			{ "Name", "Glork" },
		};

		navigationService.VerifyNavigation(
			nav => nav.NavigateAsync("/NavigationPage/HomePage", expectedParams),
			Times.Once());
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageViaUriWithParameters()
	{
		// Arrange

		// Act
		await Sut.NavigateToAbsoluteHomePageViaUriWithParameters();

		// Assert
		var expectedUri = new Uri("/NavigationPage/HomePage", UriKind.Relative);
		var expectedParams = new NavigationParameters()
		{
			{ "Name", "Glork" },
		};

		navigationService.VerifyNavigation(
			nav => nav.NavigateAsync(expectedUri, expectedParams),
			Times.Once());
	}

	#endregion Tests
}
