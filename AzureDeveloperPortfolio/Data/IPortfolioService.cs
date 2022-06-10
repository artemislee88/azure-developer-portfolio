namespace AzureDeveloperPortfolio.Data
{
	public interface IPortfolioService
	{
		Task<string> CreateProjectAsync(Project project);
	}
}
