using AzureDeveloperPortfolio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AzureDeveloperPortfolio.Tests
{
	public class TestDbContextFactory : IDbContextFactory<PortfolioContext>
	{
		private readonly DbContextOptions<PortfolioContext> _options;
		private readonly IConfiguration _config;
		public TestDbContextFactory()
		{
			ConfigurationBuilder? builder = new();
			_config = new ConfigurationBuilder()
				.AddUserSecrets("bea70df8-faf8-4154-af0d-a9aecad373d5")
				.Build();

			_options = new DbContextOptionsBuilder<PortfolioContext>()
				.UseCosmos(
				_config["Cosmos:EndPoint"],
				_config["Cosmos:AccessKey"],
				_config["Cosmos:DatabaseName"]
				?? throw new InvalidOperationException("Connection string 'PortfolioContext' not found."))
				.Options;

			using PortfolioContext? context = CreateDbContext();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

		}

		public PortfolioContext CreateDbContext()
		{
			return new PortfolioContext(_options);
		}

	}
}
