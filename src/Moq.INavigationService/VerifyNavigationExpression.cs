using System.Linq.Expressions;

namespace Moq;

internal class VerifyNavigationExpression
{
    public required VerifyNavigationExpressionArgs Args { get; set; }
    public required Expression DestinationStringExpression { get; set; }
    public required Expression DestinationUriExpression { get; set; }
    public required Expression NavigationParametersExpression { get; set; }

    public static VerifyNavigationExpression From(Expression expression)
        => expression.ToString().Contains("CreateBuilder")
            ? ParseNavigationBuilderExpression(expression)
            : ParseUriNavigationExpression(expression);

    private static VerifyNavigationExpression ParseUriNavigationExpression(Expression expression)
        => new VerifyNavigationExpression
        {
            Args = VerifyNavigationExpressionArgs.FromUriExpression(expression),
            DestinationStringExpression = ExpressionInspector.GetArgExpressionOf<string>(expression),
            DestinationUriExpression = ExpressionInspector.GetArgExpressionOf<Uri>(expression),
            NavigationParametersExpression = ExpressionInspector.GetArgExpressionOf<NavigationParameters>(expression),
        };

    private static VerifyNavigationExpression ParseNavigationBuilderExpression(Expression expression)
    {
        var args = VerifyNavigationExpressionArgs.FromNavigationBuilderExpression(expression);

        return new VerifyNavigationExpression()
        {
            Args = args,
            DestinationStringExpression = null,
            DestinationUriExpression = null,
            NavigationParametersExpression = null,
        };
    }
}
