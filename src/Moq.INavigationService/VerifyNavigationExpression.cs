using System.Linq.Expressions;
namespace Moq;

internal class VerifyNavigationExpression
{
	public required NavigationExpressionArgs Args { get; set; }
	public required Expression? DestinationStringExpression { get; set; }
	public required Expression? DestinationUriExpression { get; set; }
	public required Expression? NavigationParametersExpression { get; set; }

	public static VerifyNavigationExpression FromNavigateExpression(Expression expression)
	{
		return expression.ToString().Contains("CreateBuilder")
			? ParseNavigationBuilderExpression(expression)
			: ParseUriNavigationExpression(expression);
	}

	public static VerifyNavigationExpression FromGoBackToExpression(Expression expression)
	{
		return new VerifyNavigationExpression
		{
			Args = NavigationExpressionArgs.FromGoBackToExpression(expression),
			DestinationStringExpression = ExpressionInspector.GetArgExpressionOf<string>(expression),
			DestinationUriExpression = null,
			NavigationParametersExpression = ExpressionInspector.GetArgExpressionOf<NavigationParameters>(expression),
		};
	}

	private static VerifyNavigationExpression ParseUriNavigationExpression(Expression expression)
	{
		return new VerifyNavigationExpression
		{
			Args = NavigationExpressionArgs.FromNavigateUriExpression(expression),
			DestinationStringExpression = ExpressionInspector.GetArgExpressionOf<string>(expression),
			DestinationUriExpression = ExpressionInspector.GetArgExpressionOf<Uri>(expression),
			NavigationParametersExpression = ExpressionInspector.GetArgExpressionOf<NavigationParameters>(expression),
		};
	}

	private static VerifyNavigationExpression ParseNavigationBuilderExpression(Expression expression)
	{
		var args = NavigationExpressionArgs.FromNavigationBuilderExpression(expression);

		return new VerifyNavigationExpression
		{
			Args = args,
			DestinationStringExpression = null,
			DestinationUriExpression = null,
			NavigationParametersExpression = null,
		};
	}
}
