using System.Linq.Expressions;
using System.Reflection;
using Prism.Navigation.Builder;
namespace Moq;

internal class VerifyNavigationExpressionArgs
{
    public required Uri NavigationUri { get; set; }
    public INavigationParameters? NavigationParameters { get; set; }

    public static VerifyNavigationExpressionArgs FromNavigationBuilderExpression(Expression expression)
    {
        var args = ParseUriBuilderExpression(expression);

        return new VerifyNavigationExpressionArgs
        {
            NavigationUri = args.Uri,
            NavigationParameters = args.NavigationParameters,
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

    private static List<MethodCallExpression> GetMethodCallExpressionHierarchy(MethodCallExpression methodCall)
    {
        var calls = new List<MethodCallExpression>
        {
            methodCall,
        };

        var currentExpression = methodCall;

        while (currentExpression != null)
        {
            currentExpression = GetPreviousCall(currentExpression);
            ;

            if (currentExpression is not null)
            {
                calls.Add(currentExpression);
            }
        }

        return calls;
    }

    private static MethodCallExpression? GetPreviousCall(MethodCallExpression methodCall)
    {
        if (methodCall is null)
        {
            return null;
        }

        if (methodCall.Object is not null)
        {
            return methodCall.Object as MethodCallExpression;
        }

        if (methodCall.Arguments.Any())
        {
            return methodCall.Arguments.FirstOrDefault() as MethodCallExpression;
        }

        return null;
    }

    private static INavigationBuilder CreateNavigationBuilder(INavigationService navigationService)
    {
        var prismAssembly = typeof(INavigationService).Assembly;

        var prismTypes = prismAssembly.GetTypes();

        var navigationBuilderType = prismTypes.FirstOrDefault(t => t.Name == "NavigationBuilder");

        if (navigationBuilderType is null)
        {
            throw new InvalidOperationException("Unable to find prism type for NavigationBuilder");
        }

        var builder = Activator.CreateInstance(navigationBuilderType, navigationService) as INavigationBuilder;

        return builder ?? throw new InvalidOperationException("Unable to create instance of NavigationBuilder");
    }

    private static (Uri Uri, INavigationParameters? NavigationParameters) ParseUriBuilderExpression(Expression expression)
    {
        var methodCall = (MethodCallExpression)((LambdaExpression)expression).Body;

        var obj = methodCall.Object as MethodCallExpression;

        var methodCalls = GetMethodCallExpressionHierarchy(methodCall);

        var mockNavigationService = new MockNavigationService();

        var builder = CreateNavigationBuilder(mockNavigationService);

        var outerMethodNames = new List<string>
        {
            nameof(NavigationBuilderExtensions.CreateBuilder),
            nameof(INavigationService.NavigateAsync),
        };

        // Reverse calls to start from begginning for accurate simulation
        foreach (var call in methodCalls.Reverse<MethodCallExpression>())
        {
            if (outerMethodNames.Contains(call.Method.Name))
            {
                // We don't care about simulating CreateBuilder() or NavigateAsync()
                continue;
            }

            // Simulate this call on the builder to get our expected params
            switch (call.Method.Name)
            {
                case nameof(INavigationBuilder.AddSegment):
                {
                    // Check for a generic type
                    var segmentType = call.Method.GetGenericArguments().FirstOrDefault();

                    var usedGenericAddSegment = segmentType is not null;

                    if (!usedGenericAddSegment && call.Arguments.Count == 3)
                    {
                        var destinationArgument = call.Arguments[1];
                        var useModalArgument = call.Arguments[2];

                        var useModal = useModalArgument.GetExpressionValue<bool?>();
                        var destination = destinationArgument.GetExpressionValue<string>();

                        builder.AddSegment(destination, useModal);
                    }
                    else if (call.Arguments.Count == 2)
                    {
                        var useModalArgument = call.Arguments[1];

                        var useModal = useModalArgument.GetExpressionValue<bool>();

                        builder.AddSegment(segmentType.Name, useModal);
                    }
                    else
                    {
                        builder.AddSegment(segmentType.Name);
                    }

                    break;
                }

                case nameof(INavigationBuilder.WithParameters):
                {
                    var argument = call.Arguments.FirstOrDefault() as ListInitExpression;

                    var parameters = argument.GetExpressionValue<INavigationParameters>();

                    if (parameters is null)
                    {
                        throw new NotSupportedException($"Could not parse method call arguments as {nameof(INavigationParameters)}");
                    }

                    builder.WithParameters(parameters);

                    break;
                }

                case nameof(INavigationBuilder.AddParameter):
                {
                    var keyArgument = call.Arguments[0];
                    var valueArgument = call.Arguments[1];

                    var keyValue = keyArgument.GetExpressionValue<string>();
                    var valueValue = valueArgument.GetExpressionValue<object?>();

                    builder.AddParameter(keyValue, valueValue);

                    break;
                }

                case nameof(NavigationBuilderExtensions.AddNavigationPage):
                {
                    if (call.Arguments.Count == 1)
                    {
                        builder.AddNavigationPage();
                    }
                    else if (call.Arguments.Count == 2)
                    {
                        var useModalArgument = call.Arguments[1];
                        var useModal = useModalArgument.GetExpressionValue<bool>();

                        builder.AddNavigationPage(useModal);
                    }
                    else
                    {
                        throw new NotSupportedException("This pathway has not been implemented yet, please raise an issue with your navigation code");
                    }
                    break;
                }

                case nameof(INavigationBuilder.AddTabbedSegment):
                {
                    throw new NotSupportedException("This api has not been mapped and is not supported");
                }
            }
        }

        return (builder.Uri, GetBuilderNavigationParameters(builder));
    }

    private static INavigationParameters? GetBuilderNavigationParameters(INavigationBuilder navigationBuilder)
    {
        var navParametersField = navigationBuilder.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(INavigationParameters));

        if (navParametersField is null)
        {
            throw new InvalidOperationException("Unable to reflect & find _navigationParameters field");
        }

        var fieldValue = navParametersField.GetValue(navigationBuilder) as INavigationParameters;

        return fieldValue;
    }

    private static Uri GetNavigationUriFrom(Expression expression)
    {
        var methodCall = (MethodCallExpression)((LambdaExpression)expression).Body;

        var destination = methodCall.Arguments[1];

        if (destination.Type == typeof(Uri))
        {
            return ExpressionInspector.GetArgOf<Uri>(expression);
        }

        if (destination.Type == typeof(string))
        {
            var destinationString = ExpressionInspector.GetArgOf<string>(expression);

            if (Uri.TryCreate(destinationString, UriKind.RelativeOrAbsolute, out var destinationUri))
            {
                return destinationUri;
            }

            throw new NotSupportedException($"Could not parse destination string as uri: {destinationString}");
        }

        throw new NotSupportedException("Could not determine navigation destination from expression");
    }
}
