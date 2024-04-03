using Moq.Tests.Samples;
namespace Moq.Tests;

public class VerifyBuilderViewModelTests : FixtureBase<SampleBuilderViewModel>
{
	#region Setup

	private readonly MockNavigationService navigationService;

	public VerifyBuilderViewModelTests()
	{
		navigationService = new MockNavigationService();

		navigationService.SetupAllNavigationReturns(true);
	}

	public override SampleBuilderViewModel CreateSystemUnderTest()
	{
		return new SampleBuilderViewModel(navigationService);
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

	[Fact]
	public async Task NavigateToHomePageUsingRelativeNavigation_WhenCalled_ShouldVerify()
	{
		// Arrange

		// Act
		await Sut.NavigateToHomePageUsingRelativeNavigation();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.CreateBuilder()
				.UseRelativeNavigation()
				.AddSegment<HomePage>()
				.NavigateAsync(),
			Times.Once());
	}

	[Fact]
	public async Task NavigateToHomePageUsingAbsoluteNavigation_WhenCalled_ShouldVerify()
	{
		// Arrange

		// Act
		await Sut.NavigateToHomePageUsingAbsoluteNavigation();

		// Assert
		navigationService.VerifyNavigation(
			nav => nav.CreateBuilder()
				.UseAbsoluteNavigation()
				.AddSegment<HomePage>()
				.NavigateAsync(),
			Times.Once());
	}

	#endregion Tests
}
