namespace AzureDeveloperPortfolio.Data
{
	public interface IPortfolioService
	{
		Task<string> CreateProjectAsync(Project project);
		Task<Project?> GetProjectAsync(string projectUid);
		Task UpdateProjectAsync(Project project);


		Task<Tag?> GetTagAsync(string tagName);
	}
}
