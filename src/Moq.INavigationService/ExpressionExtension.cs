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

            return (T)value;
        }
        catch
        {
            return default;
        }
    }
}
