namespace AzureDeveloperPortfolio.Tests
{
	public class PortfolioServiceTests : IClassFixture<TestDbContextFactory>
	{
		public PortfolioServiceTests(TestDbContextFactory contextFactory) => ContextFactory = contextFactory;

		public TestDbContextFactory ContextFactory { get; }

		[Fact]
		public void Test1()
		{

		}
	}
}