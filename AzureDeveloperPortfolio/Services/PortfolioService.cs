using AzureDeveloperPortfolio.Data;
using Microsoft.EntityFrameworkCore;

namespace AzureDeveloperPortfolio.Services
{
	public class PortfolioService : IPortfolioService
	{
		private readonly IDbContextFactory<PortfolioContext> _contextFactory;

		public PortfolioService(IDbContextFactory<PortfolioContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}
	}
}
