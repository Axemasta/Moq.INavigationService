using Xunit.Abstractions;
namespace Moq.Tests;

public class EquivalenceHelperTests(ITestOutputHelper testOutputHelper)
{
	private readonly ITestOutputHelper testOutput = testOutputHelper;

	[Theory]
	[MemberData(nameof(EqualsTestData))]
	public void AreEquivalent_Should_DetermineCorrectly(object? a, object? b, bool expectedEquivalent)
	{
		// Arrange
		testOutput.WriteLine("Called with params:");
		testOutput.WriteLine($"ParameterA: {a}");
		testOutput.WriteLine($"ParameterB: {b}");
		testOutput.WriteLine($"ExpectedEquivalent: {expectedEquivalent}");

		// Act
		var equivalent = EquivalenceHelper.AreEquivalent(a, b);

		// Assert
		Assert.Equal(expectedEquivalent, equivalent);
	}

	public static IEnumerable<object[]> EqualsTestData
	{
		get
		{
			// Both are null, expect true
			yield return new object[]
			{
				null!,
				null!,
				true,
			};

			//// Both object are Uri, same value, expect true
			//yield return new object[]
			//{
			//    new Uri("navigation://NavigationPage/HomePage"),
			//    new Uri("navigation://NavigationPage/HomePage"),
			//    true,
			//};

			//// One object is Uri, other is null, expect false
			//yield return new object[]
			//{
			//    new Uri("navigation://NavigationPage/HomePage"),
			//    null!,
			//    false,
			//};

			//// One object is Uri, other is null, expect false
			//yield return new object[]
			//{
			//    null!,
			//    new Uri("navigation://NavigationPage/HomePage"),
			//    false,
			//};

			//// Both object are Uri, different values, expect false
			//yield return new object[]
			//{
			//    new Uri("navigation://NavigationPage/HomePage"),
			//    new Uri("https://www.github.com"),
			//    false,
			//};

			//// Both object are NavigationParameters, same values, expect true
			//yield return new object[]
			//{
			//    new NavigationParameters()
			//    {
			//        { "KeyOne", "Hello World" },
			//    },
			//    new NavigationParameters()
			//    {
			//        { "KeyOne", "Hello World" },
			//    },
			//    true
			//};

			//// Both object are NavigationParameters, different values, expect false
			//yield return new object[]
			//{
			//    new NavigationParameters()
			//    {
			//        { "KeyOne", "Hello World" },
			//    },
			//    new NavigationParameters()
			//    {
			//        { "ThisIsANumber", 1928 },
			//    },
			//    false,
			//};

			//// One object is INavigationParameters, other is null, expect false
			//yield return new object[]
			//{
			//    new NavigationParameters()
			//    {
			//        { "KeyOne", "Hello World" },
			//    },
			//    null!,
			//    false,
			//};
		}
	}
}
