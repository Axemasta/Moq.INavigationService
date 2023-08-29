using System.Linq.Expressions;
namespace Moq;

internal class VerifyNavigationExpression
{
    public required NavigationExpressionArgs Args { get; set; }
    public required Expression DestinationStringExpression { get; set; }
    public required Expression DestinationUriExpression { get; set; }
    public required Expression NavigationParametersExpression { get; set; }

    public static VerifyNavigationExpression From(Expression expression)
    {
        return expression.ToString().Contains("CreateBuilder")
            ? ParseNavigationBuilderExpression(expression)
            : ParseUriNavigationExpression(expression);
    }

    private static VerifyNavigationExpression ParseUriNavigationExpression(Expression expression)
    {
        return new VerifyNavigationExpression
        {
            Args = NavigationExpressionArgs.FromUriExpression(expression),
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
