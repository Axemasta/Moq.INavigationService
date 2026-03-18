namespace Moq.INavigationServiceTests.TestHelpers;

public abstract class FixtureBase<TSut>
{
	private Lazy<TSut> SutLazy { get; }

	protected TSut Sut => SutLazy.Value;

	public FixtureBase()
	{
		SutLazy = new Lazy<TSut>(CreateSystemUnderTest, LazyThreadSafetyMode.ExecutionAndPublication);
	}

	public abstract TSut CreateSystemUnderTest();
}
