using System.Linq.Expressions;
using System.Reflection;
using System.Text;
namespace Moq;

/// <summary>
/// Setup Methods For The Mock <see cref="INavigationService" />
/// </summary>
public static class VerifyNavigationExtensions
{
	#region VerifyNavigation API

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	/// <param name="failMessage">The message to display if the expression is not matched</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, string failMessage)
	{
		Verify(navigationServiceMock, expression, null, null, failMessage);
	}

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	/// <param name="times">The expected times the expression should have been matched</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, Times times)
	{
		Verify(navigationServiceMock, expression, times, null, string.Empty);
	}

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	/// <param name="times">The expected times the expression should have been matched</param>
	/// <param name="failMessage">The message to display if the expression is not matched</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, Times times, string failMessage)
	{
		Verify(navigationServiceMock, expression, times, null, failMessage);
	}

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	/// <param name="times">The expected times the expression should have been matched</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, Func<Times> times)
	{
		Verify(navigationServiceMock, expression, null, times, string.Empty);
	}

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	/// <param name="times">The expected times the expression should have been matched</param>
	/// <param name="failMessage">The message to display if the expression is not matched</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, Func<Times> times, string failMessage)
	{
		Verify(navigationServiceMock, expression, null, times, failMessage);
	}

	/// <summary>
	/// Verify Navigation Expression Was Matched
	/// </summary>
	/// <param name="navigationServiceMock">The mock navigation service</param>
	/// <param name="expression">The expression to verify</param>
	public static void VerifyNavigation(this Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression)
	{
		Verify(navigationServiceMock, expression, null, null, string.Empty);
	}

	#endregion VerifyNavigation API

	private static void Verify(Mock<INavigationService> navigationServiceMock, Expression<Action<INavigationService>> expression, Times? times, Func<Times>? timesFunc, string failMessage)
	{
		GuardVerifyExpressionIsForNavigationExtensions(expression);

		if (string.IsNullOrEmpty(failMessage))
		{
			failMessage = "Verification failed";
		}

		try
		{
			var verifyNavigationExpression = VerifyNavigationExpression.From(expression);
			var verifyExpression = CreateMoqVerifyExpressionFrom(verifyNavigationExpression);

			try
			{
				if (timesFunc is not null)
				{
					navigationServiceMock.Verify(verifyExpression, timesFunc, failMessage);
				}
				else if (times.HasValue)
				{
					navigationServiceMock.Verify(verifyExpression, times.Value, failMessage);
				}
				else
				{
					navigationServiceMock.Verify(verifyExpression, failMessage);
				}
			}
			catch (MockException mex)
			{
				throw new VerifyNavigationException(BuildExceptionMessage(mex, expression), mex);
			}
		}
		catch (NotSupportedException)
		{
			throw;
		}
		catch (VerifyNavigationException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new VerifyNavigationUnexpectedException("Moq.INavigationService encountered an unexpected exception.", ex);
		}
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

	private static Expression<Action<INavigationService>> CreateMoqVerifyExpressionFrom(VerifyNavigationExpression verifyNavigationExpression)
	{
		var navigationUriExpression = CreateNavigationUriExpression(verifyNavigationExpression);
		var navigationParametersExpression = CreateNavigationParametersExpression(verifyNavigationExpression);

		var navMethodInfo = typeof(INavigationService).GetMethod(nameof(INavigationService.NavigateAsync))!;

		var navParameter = Expression.Parameter(typeof(INavigationService), "navigationService");

		var navCallExpression = Expression.Call(navParameter, navMethodInfo,
			navigationUriExpression,
			navigationParametersExpression);

		var verifyExpression = Expression.Lambda<Action<INavigationService>>(navCallExpression, navParameter);

		return verifyExpression;
	}

	private static ConstantExpression CreateNavigationUriExpression(VerifyNavigationExpression verifyNavigationExpression)
	{
		return Expression.Constant(verifyNavigationExpression.Args.NavigationUri);
	}

	private static Expression CreateNavigationParametersExpression(VerifyNavigationExpression verifyNavigationExpression)
	{
		if (verifyNavigationExpression.NavigationParametersExpression is null && verifyNavigationExpression.Args.NavigationParameters is null)
		{
			return Expression.Call(typeof(It), "IsAny",
			[
				typeof(INavigationParameters),
			]);
		}

		return Expression.Constant(verifyNavigationExpression.Args.NavigationParameters);
	}

	private static string BuildExceptionMessage(MockException ex, Expression expression)
	{
		var stringBuilderExtensionsType = typeof(Mock).Assembly.GetTypes().First(c => c.Name == "StringBuilderExtensions");
		var appendExpressionMethod =
			stringBuilderExtensionsType.GetMethod("AppendExpression", BindingFlags.Static | BindingFlags.Public);
		var stringBuilder = new StringBuilder();
		appendExpressionMethod!.Invoke(null, new object[]
		{
			stringBuilder,
			expression,
		});
		var expressionText = stringBuilder.ToString();

		var moqIndications = ex.Message.Split(':')[0];

		return $"{moqIndications}: {expressionText}" +
				$"{Environment.NewLine}" +
				$"{Environment.NewLine}";
	}
}
