using Prism.Common;
namespace Moq;

public class MockNavigationService : Mock<INavigationService>, INavigationService, IRegistryAware
{
    public bool UsingNavigationBuilder { get; set; }

    public MockNavigationService()
    {
        Registry = new MockViewRegistry();

        var mockResult = new Mock<INavigationResult>();

        mockResult.SetupGet(m => m.Success)
            .Returns(false);

        Setup(m => m.NavigateAsync(It.IsAny<Uri>(), It.IsAny<INavigationParameters>()))
            .ReturnsAsync(mockResult.Object);
    }

    #region INavigationService

    public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
    {
        return Object.GoBackAsync(parameters);
    }

    public Task<INavigationResult> GoBackToAsync(string name, INavigationParameters parameters)
    {
        return Object.GoBackToAsync(name, parameters);
    }

    public Task<INavigationResult> GoBackToRootAsync(INavigationParameters parameters)
    {
        return Object.GoBackToRootAsync(parameters);
    }

    public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
    {
        return Object.NavigateAsync(uri, parameters);
    }

    #endregion INavigationService

    #region IRegistryAware

    public IViewRegistry Registry { get; }

    #endregion IRegistryAware

    private class MockViewRegistry : IViewRegistry
    {
        public IEnumerable<ViewRegistration> Registrations { get; } = new List<ViewRegistration>
        {
            new ViewRegistration
            {
                // Otherwise we run into this exception:
                // https://github.com/PrismLibrary/Prism/blob/d8d47b8465038c18503761aaba6a68a9bdf35a0c/src/Maui/Prism.Maui/Navigation/Builder/NavigationBuilderExtensions.cs#L88C16-L88C16
                View = typeof(NavigationPage),
            },
        };

        public object CreateView(IContainerProvider container, string name)
        {
            return name;
        }

        public string GetViewModelNavigationKey(Type viewModelType)
        {
            return viewModelType.Name;
        }

        public Type GetViewType(string name)
        {
            return name.GetType();
        }

        public bool IsRegistered(string name)
        {
            return true;
        }

        public IEnumerable<ViewRegistration> ViewsOfType(Type baseType)
        {
            return Registrations;
        }
    }
}
