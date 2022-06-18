namespace AzureDeveloperPortfolio.Data
{
	public interface IPortfolioService
	{
		Task<string> CreateProjectAsync(Project project);
		Task<Project?> GetProjectAsync(string projectUid);
		Task<List<Project>?> GetFeatureProjects();
		Task UpdateProjectAsync(Project project);
		Task DeleteProjectAsync(string projectUid);
		Task<Tag?> GetTagAsync(string tagName);
		Task<List<Tag>?> GetTagsAsync();
		Task<List<ProjectSummary>?> QueryProjectsByTagAsync(List<string> tags);
	}
}
