using System.Linq.Expressions;
using Moq.Language.Flow;
namespace Moq;

/// <summary>
/// Setup Methods For The Mock <see cref="INavigationService"/>
/// </summary>
public static class SetupNavigationExtensions
{
	#region Public API

	/// <summary>
	/// This setup method will ensure all navigation calls will return the given result.
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="result">The result that should be returned</param>
	public static void SetupAllNavigationReturns(this Mock<INavigationService> navigationServiceMock, bool result)
	{
		var mockResult = new Mock<INavigationResult>();

		mockResult.SetupGet(m => m.Success)
			.Returns(result);

		navigationServiceMock.Setup(m => m.NavigateAsync(It.IsAny<Uri>(), It.IsAny<INavigationParameters>()))
			.ReturnsAsync(mockResult.Object);
	}

	/// <summary>
	/// This setup method will ensure all navigation calls will return a failed <see cref="INavigationResult"/> with the given exception.
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="exception">The exception that caused the navigation failure</param>
	public static void SetupAllNavigationFails(this Mock<INavigationService> navigationServiceMock, Exception exception)
	{
		var mockResult = new Mock<INavigationResult>();

		mockResult.SetupGet(m => m.Success)
			.Returns(false);

		mockResult.SetupGet(m => m.Exception)
			.Returns(exception);

		navigationServiceMock.Setup(m => m.NavigateAsync(It.IsAny<Uri>(), It.IsAny<INavigationParameters>()))
			.ReturnsAsync(mockResult.Object);
	}

	/// <summary>
	/// Setup Navigation For The Given <see cref="Uri"/>
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="destination">The destination uri</param>
	/// <returns>Moq Setup</returns>
	public static ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Uri destination)
	{
		return ApplySetup(navigationServiceMock, destination, null);
	}

	/// <summary>
	/// Setup Navigation For The Given <see cref="Uri"/> &amp; <see cref="INavigationParameters"/>
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="destination">The destination uri</param>
	/// <param name="navigationParameters">The expected parameters</param>
	/// <returns>Moq Setup</returns>
	public static ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Uri destination, INavigationParameters navigationParameters)
	{
		return ApplySetup(navigationServiceMock, destination, navigationParameters);
	}

	/// <summary>
	/// Setup Navigation For The Given Destination String
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="destination">The destination string</param>
	/// <returns>Moq Setup</returns>
	/// <exception cref="NotSupportedException">This will be thrown if the string is not a valid <see cref="Uri"/></exception>
	public static ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, string destination)
	{
		if (!Uri.TryCreate(destination, UriKind.RelativeOrAbsolute, out var destinationUri))
		{
			throw new NotSupportedException("Destination could not be verified as a Uri");
		}

		return ApplySetup(navigationServiceMock, destinationUri, null);
	}

	/// <summary>
	/// Setup Navigation For The Given Destination String &amp; <see cref="INavigationParameters"/>
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="destination">The destination string</param>
	/// <param name="navigationParameters">The expected parameters</param>
	/// <returns>Moq Setup</returns>
	/// <exception cref="NotSupportedException">This will be thrown if the string is not a valid <see cref="Uri"/></exception>
	public static ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, string destination, INavigationParameters navigationParameters)
	{
		if (!Uri.TryCreate(destination, UriKind.RelativeOrAbsolute, out var destinationUri))
		{
			throw new NotSupportedException("Destination could not be verified as a Uri");
		}

		return ApplySetup(navigationServiceMock, destinationUri, navigationParameters);
	}

	/// <summary>
	/// Setup Navigation For The Navigation Builder
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The navigation builder expression that is expected</param>
	/// <returns>Moq Setup</returns>
	public static ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression)
	{
		GuardVerifyExpressionIsForNavigationExtensions(expression);

		// Workout uri
		var verifyNavigationExpression = VerifyNavigationExpression.From(expression);

		return ApplySetup(
			navigationServiceMock,
			verifyNavigationExpression?.Args?.NavigationUri,
			verifyNavigationExpression?.Args?.NavigationParameters);
	}

	#endregion Public API

	private static ISetup<INavigationService, Task<INavigationResult>> ApplySetup(Mock<INavigationService> navigationServiceMock, Uri? uri, INavigationParameters? navigationParameters)
	{
		return navigationServiceMock.Setup(
			m => m.NavigateAsync(
				It.Is<Uri>(u => EquivalenceHelper.AreEquivalent(u, uri)),
				It.Is<INavigationParameters>(n => EquivalenceHelper.AreEquivalent(n, navigationParameters))));
	}

	private static void GuardVerifyExpressionIsForNavigationExtensions(Expression expression)
	{
		var methodCall = (expression as LambdaExpression)?.Body as MethodCallExpression;
		var methodName = methodCall?.Method.Name ?? throw new InvalidOperationException("Could not determine calling method name");

		if (!methodName.Equals(nameof(INavigationService.NavigateAsync), StringComparison.Ordinal))
		{
			throw new NotSupportedException($"Calling method named {methodName} is not supported, only {nameof(INavigationService.NavigateAsync)} needs to use mock expressions.");
		}
	}
}
