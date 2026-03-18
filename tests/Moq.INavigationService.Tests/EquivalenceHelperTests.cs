namespace Moq.INavigationServiceTests;

public class EquivalenceHelperTests(ITestOutputHelper testOutputHelper)
{
	[Theory]
	[MemberData(nameof(EqualsTestData))]
	public void AreEquivalent_Should_DetermineCorrectly(object? a, object? b, bool expectedEquivalent)
	{
		// Arrange
		testOutputHelper.WriteLine("Called with params:");
		testOutputHelper.WriteLine($"ParameterA: {a}");
		testOutputHelper.WriteLine($"ParameterB: {b}");
		testOutputHelper.WriteLine($"ExpectedEquivalent: {expectedEquivalent}");

		// Act
		var equivalent = EquivalenceHelper.AreEquivalent(a, b);

		// Assert
		Assert.Equal(expectedEquivalent, equivalent);
	}

	public static TheoryData<object?, object?, bool> EqualsTestData => new()
	{
		{
			// Both are null, expect true
			null, null, true
		},
		{
			// Both object are Uri, same value, expect true
			new Uri("navigation://NavigationPage/HomePage"), new Uri("navigation://NavigationPage/HomePage"), true
		},
		{
			// One object is Uri, other is null, expect false
			new Uri("navigation://NavigationPage/HomePage"), null, false
		},
		{
			// One object is Uri, other is null, expect false
			null, new Uri("navigation://NavigationPage/HomePage"), false
		},
		{
			// Both object are Uri, different values, expect false
			new Uri("navigation://NavigationPage/HomePage"), new Uri("https://www.github.com"), false
		},
		{
			// Both object are NavigationParameters, same values, expect true
			new NavigationParameters
			{
				{
					"KeyOne", "Hello World"
				},
			},
			new NavigationParameters
			{
				{
					"KeyOne", "Hello World"
				},
			},
			true
		},
		{
			// Both object are NavigationParameters, different values, expect false
			new NavigationParameters
			{
				{
					"KeyOne", "Hello World"
				},
			},
			new NavigationParameters
			{
				{
					"ThisIsANumber", 1928
				},
			},
			false
		},
		{
			// One object is INavigationParameters, other is null, expect false
			new NavigationParameters
			{
				{
					"KeyOne", "Hello World"
				},
			},
			null, false
		},
	};
}
