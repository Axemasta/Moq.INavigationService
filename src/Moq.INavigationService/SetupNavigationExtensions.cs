using System.Linq.Expressions;

namespace Moq;

public static class SetupNavigationExtensions
{
    #region Public API

    public static void SetupAllNavigationReturns(this Mock<INavigationService> navigationServiceMock, bool result)
    {
        var mockResult = new Mock<INavigationResult>();

        mockResult.SetupGet(m => m.Success)
            .Returns(result);

        navigationServiceMock.Setup(m => m.NavigateAsync(It.IsAny<Uri>(), It.IsAny<INavigationParameters>()))
            .ReturnsAsync(mockResult.Object);
    }

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

    public static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Uri destination)
    {
        return ApplySetup(navigationServiceMock, destination, null);
    }

    public static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Uri destination, INavigationParameters navigationParameters)
    {
        return ApplySetup(navigationServiceMock, destination, navigationParameters);
    }

    public static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, string destination)
    {
        if (!Uri.TryCreate(destination, UriKind.RelativeOrAbsolute, out var destinationUri))
        {
            throw new NotSupportedException("Destination could not be verified as a Uri");
        }

        return ApplySetup(navigationServiceMock, destinationUri, null);
    }

    public static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, string destination, INavigationParameters navigationParameters)
    {
        if (!Uri.TryCreate(destination, UriKind.RelativeOrAbsolute, out var destinationUri))
        {
            throw new NotSupportedException("Destination could not be verified as a Uri");
        }

        return ApplySetup(navigationServiceMock, destinationUri, navigationParameters);
    }

    public static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> SetupNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression)
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

    private static Language.Flow.ISetup<INavigationService, Task<INavigationResult>> ApplySetup(Mock<INavigationService> navigationServiceMock, Uri? uri, INavigationParameters? navigationParameters)
    {
        return navigationServiceMock.Setup(
            m => m.NavigateAsync(
                It.Is<Uri>(u => EquivalenceHelper.AreEquivalent(u, uri)),
                It.Is<INavigationParameters>(n => EquivalenceHelper.AreEquivalent(n, navigationParameters))));
    }

    private static void GuardVerifyExpressionIsForNavigationExtensions(Expression expression)
    {
        var methodCall = (expression as LambdaExpression)?.Body as MethodCallExpression;
        var methodName = methodCall?.Method.Name;

        if (methodName is null)
        {
            throw new InvalidOperationException("Could not determine calling method name");
        }

        if (!methodName.Equals(nameof(INavigationService.NavigateAsync)))
        {
            throw new NotSupportedException($"Calling method named {methodName} is not supported, only {nameof(INavigationService.NavigateAsync)} needs to use mock expressions.");
        }
    }
}
