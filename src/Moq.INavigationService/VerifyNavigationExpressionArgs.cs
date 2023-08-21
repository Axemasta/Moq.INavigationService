using System.Linq.Expressions;

namespace Moq;

internal class VerifyNavigationExpressionArgs
{
	public required Uri NavigationUri { get; set; }
	public INavigationParameters? NavigationParameters { get; set; }

    public static VerifyNavigationExpressionArgs FromNavigationBuilderExpression(Expression expression)
    {
        return new VerifyNavigationExpressionArgs
        {
            NavigationUri = GetBuilderUriFrom(expression),
            NavigationParameters = ExpressionInspector.GetArgOf<NavigationParameters>(expression),
        };
    }

    public static VerifyNavigationExpressionArgs FromUriExpression(Expression expression)
    {
        return new VerifyNavigationExpressionArgs
        {
            NavigationUri = GetNavigationUriFrom(expression),
            NavigationParameters = ExpressionInspector.GetArgOf<NavigationParameters>(expression),
        };
    }

    private static Uri GetBuilderUriFrom(Expression expression)
    {
        var methodCall = (MethodCallExpression)((LambdaExpression)expression).Body;

        return new Uri("HomePage", UriKind.Relative);
    }

    private static Uri GetNavigationUriFrom(Expression expression)
    {
        var methodCall = (MethodCallExpression)((LambdaExpression)expression).Body;

        var destination = methodCall.Arguments[1];

        if (destination.Type == typeof(Uri))
        {
            return ExpressionInspector.GetArgOf<Uri>(expression);
        }
        else if (destination.Type == typeof(string))
        {
            var destinationString = ExpressionInspector.GetArgOf<string>(expression);

            if (Uri.TryCreate(destinationString, UriKind.RelativeOrAbsolute, out Uri? destinationUri))
            {
                return destinationUri;
            }

            throw new NotSupportedException($"Could not parse destination string as uri: {destinationString}");
        }
        else
        {
            throw new NotSupportedException("Could not determine navigation destination from expression");
        }
    }
}

