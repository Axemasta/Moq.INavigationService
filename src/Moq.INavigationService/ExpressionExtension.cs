using System.Linq.Expressions;
namespace Moq;

internal static class ExpressionExtension
{
	public static T? GetExpressionValue<T>(this Expression expression)
	{
		try
		{
			var value = Expression.Lambda(expression)
				.Compile()
				.DynamicInvoke();

			if (value is null)
			{
				return default;
			}

			return (T)value;
		}
		catch
		{
			return default;
		}
	}

	public static string GetExpressionMethodName(this Expression expression)
	{
		var methodCall = (expression as LambdaExpression)?.Body as MethodCallExpression;
		var methodName = methodCall?.Method.Name ?? throw new InvalidOperationException("Could not determine calling method name");

		return methodName;
	}
}
