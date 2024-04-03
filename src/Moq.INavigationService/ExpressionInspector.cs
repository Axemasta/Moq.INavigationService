using System.Linq.Expressions;
namespace Moq;

internal static class ExpressionInspector
{
	internal static T GetArgOf<T>(Expression expression) where T : class
	{
		var arg = GetArgExpression(expression, c => c.Type == typeof(T));

		if (arg is null)
		{
			return null!;
		}

		if (arg is ConstantExpression cexp)
		{
			return cexp.Value as T ?? throw new InvalidCastException($"Unable to cast {cexp.Value} as {nameof(T)}");
		}

		var val = arg.GetExpressionValue<T>();

		return val ?? throw new InvalidCastException($"Unable to cast {val} as {nameof(T)}");
	}

	internal static Expression GetArgExpression(Expression expression, Func<Expression, bool> argPredicate)
	{
		var methodCall = (MethodCallExpression)((LambdaExpression)expression).Body;
		var argExpression = methodCall.Arguments.FirstOrDefault(argPredicate);
		return argExpression;
	}

	internal static Expression GetArgExpressionOf<T>(Expression expression)
	{
		return GetArgExpression(expression, c => c.Type == typeof(T));
	}
}
