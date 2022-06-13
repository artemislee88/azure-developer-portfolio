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

			// addedTagNames - create a List<string> of tagName(s) for added tag(s)
			List<string> addedTagNames = new();


			// If this is a Project update:
			//	Remove ProjectSummary from removed Tag(s)
			//	(conditional) Update ProjectSummary in existing Tag(s)
			if (storedProject is not null && storedProject.Tags is not null)
			{

				List<string> storedTagNames = storedProject.Tags;

				// removedTagNames - create a List<string> of tagName(s) for removed tag(s)
				List<string> removedTagNames;
				if (project.Tags is null)
				{
					removedTagNames = storedTagNames;
				}
				else
				{
					removedTagNames = storedTagNames.Except(project.Tags).ToList();
					addedTagNames = project.Tags.Except(storedTagNames).ToList();
				}

				await AddProjectSummariesToTagsAsync(context, project, addedTagNames);

				// If there are removed Tag(s), remove ProjectSummary(s)
				if (removedTagNames is not null)
				{
					await RemoveProjectSummariesFromTagsAsync(
					   context, project, removedTagNames);
				}


				// Determine if ProjectSummary has changed
				ProjectSummary projectSummary = new(project);
				bool summaryEquals = projectSummary.Equals(new ProjectSummary(storedProject));
				if (!summaryEquals)
				{
					List<string> remainingTagNames = (removedTagNames is null)
						? storedTagNames
						: storedTagNames.Except(removedTagNames).ToList();

					// Ensure "index" Tag is updated
					remainingTagNames.Add("index");
					await UpdateProjectSummariesInTagsAsync(
					   context, project, remainingTagNames);
				}
			}

			// If this is a new Project, add ProjectSummary to all Tags
			if (storedProject is null)
			{
				if (project.Tags is not null)
				{
					addedTagNames = project.Tags.Select(tag => tag).ToList();
				}
				// Ensure "index" Tag is updated
				addedTagNames.Add("index");
				await AddProjectSummariesToTagsAsync(context, project, addedTagNames);
			}
		}


		private static async Task AddProjectSummariesToTagsAsync(PortfolioContext context, Project project, IEnumerable<string> addedTagNames)
		{
			foreach (string tagName in addedTagNames)
			{
				Tag? addedTag = await context.Tags
					.WithPartitionKey(tagName)
					.SingleOrDefaultAsync();

				// If no Tag is found, create a new Tag
				if (addedTag is null)
				{
					addedTag = new Tag(tagName);
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

		private static async Task RemoveProjectSummariesFromTagsAsync(PortfolioContext context, Project project, IEnumerable<string> removedTagNames)
		{
			foreach (string removedTagName in removedTagNames)
			{
				Tag? removedTag = await context.Tags
					.Where(t => t.TagName == removedTagName)
					.FirstOrDefaultAsync();

				if (removedTag is not null)
				{
					ProjectSummary? deletedProjectSummary = removedTag.Projects
						.Find(summary => summary.ProjectUid == project.Uid);
					if (deletedProjectSummary is not null)
					{
						removedTag.Projects.Remove(deletedProjectSummary);

						// If Tag.Projects is empty, delete Tag
						if (removedTag.Projects is null)
						{
							context.Remove(removedTag);
						}
						else
						{
							context.Entry(removedTag).State = EntityState.Modified;
						}
					}
				}
			}
		}

		private static async Task UpdateProjectSummariesInTagsAsync(PortfolioContext context, Project project, IEnumerable<string> existingTagNames)
		{
			// Remove stale ProjectSummary, add updated ProjectSummary
			foreach (string existingTagName in existingTagNames)
			{
				Tag? existingTag = await context.Tags
					.Where(t => t.TagName == existingTagName)
					.FirstOrDefaultAsync();
				if (existingTag is not null)
				{
					ProjectSummary? staleProjectSummary =
						existingTag.Projects.FirstOrDefault(summary =>
							summary.ProjectUid == project.Uid);
					if (staleProjectSummary is not null)
					{
						existingTag.Projects.Remove(staleProjectSummary);
					}
					existingTag.Projects.Add(new ProjectSummary
					{
						ProjectUid = project.Uid,
						ProjectName = project.ProjectName,
						ShortDescription = project.ShortDescription,
						ProfileScreenshot = project.ProfileScreenshot,
						LastUpdated = project.LastUpdated,
					});
					context.Entry(existingTag).State = EntityState.Modified;
				}
			}
		}

	}
}
