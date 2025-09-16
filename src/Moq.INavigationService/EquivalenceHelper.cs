using Xunit;

namespace Moq;

internal class EquivalenceHelper
{
	public static bool AreEquivalent(object? a, object? b)
	{
		try
		{
			Assert.Equivalent(a, b);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
