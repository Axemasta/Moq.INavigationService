using System.Linq.Expressions;
namespace Moq;

public static class ExpressionExtension
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
}
