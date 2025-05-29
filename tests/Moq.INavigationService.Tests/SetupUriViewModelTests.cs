using Moq.Tests.Samples;
namespace Moq.Tests;

public class SetupUriViewModelTests : FixtureBase<SampleUriViewModel>
{
	#region Setup

	private readonly MockNavigationService navigationService;

	public SetupUriViewModelTests()
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
	public async Task Setup_NavigateToHomePageWhenSetupIsMatched_ShouldReturnSetup()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		navigationService.SetupNavigation("HomePage")
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePage();

		// Assert
		Assert.Equal(expectedNavigationResult, result);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageWhenSetupIsNotMatched_ShouldReturnNull()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		navigationService.SetupNavigation("NotTheHomePage")
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePage();

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Setup_NavigateToHomePageWhenSetupIsInvalid_ShouldThrow()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		// Act
		var ex = Record.Exception(() =>
		{
			string destination = null!;

			navigationService.SetupNavigation(destination)
				.ReturnsAsync(expectedNavigationResult);
		});

		// Assert
		Assert.IsType<NotSupportedException>(ex);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageViaUriWhenSetupIsMatched_ShouldReturnSetup()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		navigationService.SetupNavigation(new Uri("HomePage", UriKind.Relative))
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePageViaUri();

		// Assert
		Assert.Equal(expectedNavigationResult, result);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageViaUriWhenSetupIsNotMatched_ShouldReturnNull()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		navigationService.SetupNavigation(new Uri("SomewhereElse/Entirely", UriKind.Relative))
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePageViaUri();

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageWithParametersWhenSetupIsMatched_ShouldReturnSetup()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		var correctParameters = new NavigationParameters
		{
			{
				"KeyOne", "Hello World"
			},
		};

		navigationService.SetupNavigation("HomePage", correctParameters)
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePageWithParameters();

		// Assert
		Assert.Equal(expectedNavigationResult, result);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageWithParametersWhenSetupIsNotMatched_ShouldReturnNull()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		var incorrectParameters = new NavigationParameters
		{
			{
				"KeyTwo", 139478
			},
		};

		navigationService.SetupNavigation("HomePage", incorrectParameters)
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePage();

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Setup_NavigateToHomePageWithParametersWhenSetupIsInvalid_ShouldThrow()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		var parameters = new NavigationParameters
		{
			{
				"KeyTwo", 139478
			},
		};

		// Act
		var ex = Record.Exception(() =>
		{
			string destination = null!;

			navigationService.SetupNavigation(destination, parameters)
				.ReturnsAsync(expectedNavigationResult);
		});

		// Assert
		Assert.IsType<NotSupportedException>(ex);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageWithParametersViaUriWhenSetupIsMatched_ShouldReturnSetup()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		var correctParameters = new NavigationParameters
		{
			{
				"KeyOne", "Hello World"
			},
		};

		navigationService.SetupNavigation(new Uri("HomePage", UriKind.Relative), correctParameters)
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePageWithParametersViaUri();

		// Assert
		Assert.Equal(expectedNavigationResult, result);
	}

	[Fact]
	public async Task Setup_NavigateToHomePageWithParametersViaUriWhenSetupIsNotMatched_ShouldReturnNull()
	{
		// Arrange
		var expectedNavigationResult = new NavigationResult();

		var incorrectParameters = new NavigationParameters
		{
			{
				"KeyTwo", 139478
			},
		};

		navigationService.SetupNavigation(new Uri("SomewhereElse/Entirely", UriKind.Relative), incorrectParameters)
			.ReturnsAsync(expectedNavigationResult);

		// Act
		var result = await Sut.NavigateToHomePageWithParametersViaUri();

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePage()
	{
		// Arrange
		navigationService.SetupNavigation(nav => nav.NavigateAsync("/NavigationPage/HomePage"));

		// Act
		await Sut.NavigateToAbsoluteHomePage();

		// Assert
		navigationService.Verify();
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageViaUri()
	{
		// Arrange
		var expectedUri = new Uri("/NavigationPage/HomePage", UriKind.Relative);

		navigationService.SetupNavigation(nav => nav.NavigateAsync(expectedUri));

		// Act
		await Sut.NavigateToAbsoluteHomePageViaUri();

		// Assert
		navigationService.Verify();
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageWithParameters()
	{
		// Arrange
		var expectedParams = new NavigationParameters()
		{
			{ "Name", "Glork" },
		};

		navigationService.SetupNavigation(nav => nav.NavigateAsync("/NavigationPage/HomePage", expectedParams));

		// Act
		await Sut.NavigateToAbsoluteHomePageWithParameters();

		// Assert
		navigationService.Verify();
	}

	[Fact]
	public async Task Verify_NavigateToAbsoluteHomePageViaUriWithParameters()
	{
		// Arrange
		var expectedUri = new Uri("/NavigationPage/HomePage", UriKind.Relative);
		var expectedParams = new NavigationParameters()
		{
			{ "Name", "Glork" },
		};

		navigationService.SetupNavigation(nav => nav.NavigateAsync(expectedUri, expectedParams));

		// Act
		await Sut.NavigateToAbsoluteHomePageViaUriWithParameters();

		// Assert
		navigationService.Verify();
	}

	#endregion Tests
}
