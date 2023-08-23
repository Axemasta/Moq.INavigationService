# Moq.INavigationService

Enables [Moq's](https://github.com/moq/moq/wiki/Quickstart) **Verify** API over the `INavigationService` extensions in [Prism.Maui](https://github.com/PrismLibrary/Prism).

This package is directly inspired by, and uses the design pattern of [Moq.ILogger](https://github.com/adrianiftode/Moq.ILogger). I found the design interesting and really liked the API it exposes for testing.

[![Moq.ILogger CI](https://github.com/Axemasta/Moq.INavigationService/actions/workflows/ci.yml/badge.svg)](https://github.com/Axemasta/Moq.INavigationService/actions/workflows/ci.yml) [![NuGet Shield](https://img.shields.io/nuget/v/Axemasta.Moq.INavigationService)](https://www.nuget.org/packages/Axemasta.Moq.INavigationService/)

## Usage

To verify navigation in your tests, use the `VerifyNavigation` api & provide the expression you wish to verify:

```csharp
navigationService.VerifyNavigation(
    nav => nav.NavigateAsync("HomePage"),
    Times.Once());
```

this also works for the navigation builder:
```csharp
navigationService.VerifyNavigation(
    nav => nav.CreateBuilder()
        .AddSegment<HomePage>()
        .NavigateAsync(),
    Times.Once());
```

Not everything has been setup for mocking in this library, such as the `TabbedSegmentBuilder`, I haven't yet implemented a mock for it. I found that mocking the builder expressions extremely difficult and the implementation is far from ideal (I'm basically recreating the call and comparing the results).

If the verification expression fails, it will throw a `VerifyNavigationException`, this is a problem on your end and either your code or test is wrong.

If the a `VerifyNavigationUnknownException` is thrown, this is an issue with the library, please raise an issue with details of the exception.

## Why?

This package exists to enable unit testing of Prism.Maui's `INavigationService` due to the vast majority of the navigation methods being [Extension methods](https://github.com/PrismLibrary/Prism/blob/master/src/Maui/Prism.Maui/Navigation/INavigationServiceExtensions.cs).

In Xamarin these methods exists on the `INavigationService` interface so could be mocked directly:

### Xamarin Example:

This couldn't have been easier to test and was a primary reason Prism was so essential to Xamarin Forms apps.

**ViewModel**
```csharp
public class FooViewModel
{
    private readonly INavigationService navigationService;

    public FooViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public async Task NavigateToBar()
    {
        await navigationService.NavigateAsync("BarPage");
    }
}
```

**Test**
```csharp
[Fact]
public async Task NavigateToBar_WhenCalled_ShouldNavigateToBarPage()
{
    // Arrange
    var mockNavigationService = new Mock<INavigationService>();
    var sut = new FooViewModel(mockNavigationService.Object);

    // Act
    await sut.NavigateToBar();

    // Assert
    mockNavigationService.Verify(
        m => m.NavigateAsync("BarPage"), 
        Times.Once(),
        "Navigation to destination: BarPage should have occurred");
}
```
### Maui Example:

If we try this test in maui the following exception will be thrown when running the mock verification:

```
System.NotSupportedException : Unsupported expression: m => m.NavigateAsync("BarPage")
Extension methods (here: INavigationServiceExtensions.NavigateAsync) may not be used in setup / verification expressions.
```

The solution to this is understanding how the extension methods work, the extension will call the underlying interface navigation method:
`Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters);`

We can update our test to use this overload to make our tests pass:

```csharp
[Fact]
public async Task NavigateToBar_WhenCalled_ShouldNavigateToBarPage()
{
    // Arrange
    var mockNavigationService = new Mock<INavigationService>();
    var sut = new FooViewModel(mockNavigationService.Object);

    // Act
    await sut.NavigateToBar();

    // Assert
    mockNavigationService.Verify(
        m => m.NavigateAsync(
			It.Is<Uri>(u => u.Equals("BarPage")),
			null),
        Times.Once(),
        "Navigation to destination: BarPage should have occurred");
}
```

This approach is fine however it requires a more technical understanding of the prism library under the hood and for more complicated navigation calls or if you want to use the new `INavigationBuilder`, verification becomes a more difficult endeavour.

The purpose of this library is to make testing prism maui viewmodels easier by acting as a testing helper library. I have opened this as a [discussion](https://github.com/PrismLibrary/Prism/discussions/2850) on the main prism repo and all code is welcome to be used by prism, I am publishing this repo to NuGet due to the personal need for this library.
