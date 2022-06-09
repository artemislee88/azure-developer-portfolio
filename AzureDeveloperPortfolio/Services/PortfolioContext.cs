using Microsoft.EntityFrameworkCore;

namespace AzureDeveloperPortfolio.Services
{
	public sealed class PortfolioContext : DbContext
	{
		public PortfolioContext(DbContextOptions<PortfolioContext> options)
			: base(options)
		{ }

	}
}
