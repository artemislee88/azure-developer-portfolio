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

		public async Task<string> CreateProjectAsync(Project newProject)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			newProject.LastUpdated = new DateTime();
			if (newProject.ProfileScreenshot is null || newProject.ProfileScreenshot == string.Empty)
			{
				newProject.ProfileScreenshot = "/img/default.jpg";

			}
			await HandleTagsAsync(context, newProject);
			context.Add(newProject);
			await context.SaveChangesAsync();
			return newProject.Uid;
		}



		private static async Task HandleTagsAsync(
			PortfolioContext context, Project project)
		{
			// Retrieve stored Project data
			Project? storedProject = await context.Projects
				.AsNoTracking()
				.WithPartitionKey(project.Uid)
				.SingleOrDefaultAsync(p => p.Uid == project.Uid);


			// Create Project Summary for modifiedProject
			ProjectSummary projectSummary = new()
			{
				ProjectUid = project.Uid,
				ProjectName = project.ProjectName,
				ShortDescription = project.ShortDescription,
				ProfileScreenshot = project.ProfileScreenshot,
			};

			bool projectSummaryChanged = (storedProject is null);


			// addedTagNames - create a IEnumerable<string> of tagName(s) for added tag(s)
			List<string>? addedTagNames = storedProject is null
				? project.Tags
				: project.Tags.Where(
					t => !project.Tags.Any(rt => rt == t))
				.ToList();

			if (!addedTagNames.Contains("index"))
			{
				addedTagNames.Add("index");
			}
			await AddProjectSummariesToTagsAsync(context, project, addedTagNames);

		}


		private static async Task AddProjectSummariesToTagsAsync(PortfolioContext context, Project project, IEnumerable<string> tagNames)
		{
			foreach (string? tag in tagNames)
			{
				Tag? addedTag = await context.Tags
					.WithPartitionKey(tag)
					.SingleOrDefaultAsync();

				// If no Tag is found, create a new Tag
				if (addedTag is null)
				{
					addedTag = new Tag(tag);
					await context.Tags.AddAsync(addedTag);
				}
				else
				{
					context.Entry(addedTag).State = EntityState.Modified;
				}
				addedTag.Projects.Add(new ProjectSummary
				{
					ProjectUid = project.Uid,
					ProjectName = project.ProjectName,
					ShortDescription = project.ShortDescription,
					ProfileScreenshot = project.ProfileScreenshot,
					LastUpdated = project.LastUpdated,
				});
			}

		}
	}
}
