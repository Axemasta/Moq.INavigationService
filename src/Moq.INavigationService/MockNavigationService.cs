using Prism.Common;
namespace Moq;

/// <summary>
/// Mock Navigation Service, Capable of mocking the <see cref="PageNavigationService" /> during a unit
/// test.
/// </summary>
public class MockNavigationService : Mock<INavigationService>, INavigationService, IRegistryAware
{
	#region INavigationService

	/// <inheritdoc />
	public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
	{
		return Object.GoBackAsync(parameters);
	}

	/// <inheritdoc />
	public Task<INavigationResult> GoBackToAsync(string viewName, INavigationParameters parameters)
	{
		return Object.GoBackToAsync(viewName, parameters);
	}

	/// <inheritdoc />
	public Task<INavigationResult> GoBackToRootAsync(INavigationParameters parameters)
	{
		return Object.GoBackToRootAsync(parameters);
	}

	/// <inheritdoc />
	public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
	{
		return Object.NavigateAsync(uri, parameters);
	}

	/// <inheritdoc />
	public Task<INavigationResult> SelectTabAsync(string name, Uri uri, INavigationParameters parameters)
	{
		return Object.SelectTabAsync(name, uri, parameters);
	}

	#endregion INavigationService

	#region IRegistryAware

	/// <inheritdoc />
	public IViewRegistry Registry { get; } = new MockViewRegistry();

	#endregion IRegistryAware

	private class MockViewRegistry : IViewRegistry
	{
		public IEnumerable<ViewRegistration> Registrations { get; } = new List<ViewRegistration>
		{
			new()
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
