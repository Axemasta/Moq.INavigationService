using KellermanSoftware.CompareNetObjects;

namespace Moq;

internal sealed class EquivalenceHelper
{
	private static readonly CompareLogic CompareLogic = new();

	public static bool AreEquivalent(object? a, object? b)
	{
		return CompareLogic.Compare(a, b).AreEqual;
	}
}
