using FluentAssertions;

namespace Moq;

internal class EquivalenceHelper
{
    public static bool AreEquivalent(object? a, object? b)
    {
        try
        {
            a.Should().BeEquivalentTo(b);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
