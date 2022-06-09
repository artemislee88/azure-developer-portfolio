using AzureDeveloperPortfolio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AzureDeveloperPortfolio.Tests
{
	public class TestDbContextFactory : IDbContextFactory<PortfolioContext>
	{
		private static readonly object _lock = new();
		private static bool _databaseInitialized;

		private readonly DbContextOptions<PortfolioContext> _options;

		public TestDbContextFactory()
		{
			ConfigurationBuilder? builder = new();
			IConfigurationRoot? config = new ConfigurationBuilder()
				.AddUserSecrets("bea70df8-faf8-4154-af0d-a9aecad373d5")
				.Build();

			_options = new DbContextOptionsBuilder<PortfolioContext>()
				.UseCosmos(
				config["Cosmos:EndPoint"],
				config["Cosmos:AccessKey"],
				config["Cosmos:DatabaseName"]
				?? throw new InvalidOperationException("Connection string 'PortfolioContext' not found."))
				.Options;

			lock (_lock)
			{
				if (!_databaseInitialized)
				{
					using (PortfolioContext? context = CreateDbContext())
					{
						context.Database.EnsureDeleted();
						context.Database.EnsureCreated();

					}

					_databaseInitialized = true;
				}
			}
		}

		public PortfolioContext CreateDbContext()
		{
			return new PortfolioContext(_options);
		}

	}

}
