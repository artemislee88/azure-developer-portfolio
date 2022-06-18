using AzureDeveloperPortfolio.Data;
using Microsoft.EntityFrameworkCore;

namespace AzureDeveloperPortfolio.Services
{
	public class PortfolioService : IPortfolioService
	{
		private static readonly string Index = nameof(Index);
		private readonly IDbContextFactory<PortfolioContext> _contextFactory;

		public PortfolioService(IDbContextFactory<PortfolioContext> contextFactory) =>
			_contextFactory = contextFactory;


		public async Task<string> CreateProjectAsync(Project newProject)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			await HandleTagsAsync(context, newProject);
			context.Add(newProject);
			await context.SaveChangesAsync();
			return newProject.Uid;
		}

		public async Task<Project?> GetProjectAsync(string projectUid)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			Project? project = await context.Projects
				.WithPartitionKey(projectUid)
				.SingleOrDefaultAsync(p => p.Uid == projectUid);
			if (project is null)
			{
				return null;
			}
			return project;
		}

		public async Task<List<Project>?> GetFeatureProjects()
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			List<Project>? featured = await context.Projects
				.AsNoTracking()
				.Where(p => p.Featured == true)
				.OrderBy(p => p.LastUpdated)
				.ToListAsync();
			if (featured is null || !featured.Any())
			{
				return null;
			}
			return featured;
		}

		public async Task UpdateProjectAsync(Project updatedProject)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			Project? project = await context.Projects
				.WithPartitionKey(updatedProject.Uid)
				.SingleOrDefaultAsync(p => p.Uid == updatedProject.Uid);
			if (project is null)
			{
				return;
			}

			await HandleTagsAsync(context, updatedProject);

			project.ProjectName = updatedProject.ProjectName;
			project.ShortDescription = updatedProject.ShortDescription;
			project.Summary = updatedProject.Summary;
			project.Markdown = updatedProject.Markdown;
			project.Screenshots = updatedProject.Screenshots;
			project.ProfileScreenshot = updatedProject.ProfileScreenshot;
			project.RepositoryURL = updatedProject.RepositoryURL;
			project.SiteURL = updatedProject.SiteURL;
			project.IsDeployed = updatedProject.IsDeployed;
			project.Hosting = updatedProject.Hosting;
			project.Tags = updatedProject.Tags;
			project.Published = updatedProject.Published;
			project.Featured = updatedProject.Featured;

			await context.SaveChangesAsync();
		}

		public async Task DeleteProjectAsync(string projectUid)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			Project? project = await context.Projects
				.WithPartitionKey(projectUid)
				.SingleOrDefaultAsync(p => p.Uid == projectUid);
			if (project is null)
			{
				return;
			}
			project.Tags.Add(nameof(Index));
			await RemoveProjectSummariesFromTagsAsync(context, project.Uid, project.Tags);

			context.Remove(project);
			await context.SaveChangesAsync();
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
					   context, project.Uid, removedTagNames);
				}


				// Determine if ProjectSummary has changed
				ProjectSummary projectSummary = new(project);
				bool summaryEquals = projectSummary.Equals(new ProjectSummary(storedProject));
				if (!summaryEquals)
				{
					List<string> remainingTagNames = (removedTagNames is null)
						? storedTagNames
						: storedTagNames.Except(removedTagNames).ToList();

					// Ensure "Index" Tag is updated
					remainingTagNames.Add(nameof(Index));
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
				// Ensure "Index" Tag is updated
				addedTagNames.Add(nameof(Index));
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
					context.Tags.Add(addedTag);
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
					LastUpdated = project.LastUpdated
				});
			}
		}

		private static async Task RemoveProjectSummariesFromTagsAsync(PortfolioContext context, string projectUid, IEnumerable<string> removedTagNames)
		{
			foreach (string removedTagName in removedTagNames)
			{
				Tag? removedTag = await context.Tags
					.WithPartitionKey(removedTagName)
					.SingleOrDefaultAsync(t => t.TagName == removedTagName);

				if (removedTag is not null)
				{
					ProjectSummary? deletedProjectSummary = removedTag.Projects
						.Find(summary => summary.ProjectUid == projectUid);
					if (deletedProjectSummary is not null)
					{
						removedTag.Projects.Remove(deletedProjectSummary);

						// If Tag.Projects is empty, delete Tag
						if (!removedTag.Projects.Any())
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
					.WithPartitionKey(existingTagName)
					.SingleOrDefaultAsync(t => t.TagName == existingTagName);
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

		public async Task<Tag?> GetTagAsync(string tagName)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			Tag? tag = await context.Tags
				.WithPartitionKey(tagName)
				.SingleOrDefaultAsync(t => t.TagName == tagName);
			if (tag is null)
			{
				return null;
			}
			if (tag.Projects is not null)
			{
				List<ProjectSummary>? sorted =
					tag.Projects.OrderByDescending(p => p.LastUpdated).ToList();
				tag.Projects = sorted;
			}
			return tag;
		}

		public async Task<List<Tag>?> GetTagsAsync()
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			List<Tag>? tags = await context.Tags
				.OrderBy(t => t.TagName)
				.ToListAsync();

			if (tags is null)
			{
				return null;
			}

			return tags;
		}


		public async Task<List<ProjectSummary>?> QueryProjectsByTagAsync(List<string> tagNames)
		{
			using PortfolioContext context = _contextFactory.CreateDbContext();
			List<Tag>? tagList = await context.Tags.AsNoTracking()
				.Where(t => tagNames.Contains(t.TagName))
				.ToListAsync();

			if (tagList is null) { return null; }
			else
			{

				List<ProjectSummary> summaryResults = tagList.AsQueryable()
					.Aggregate(tagList.First().Projects,
					(current, next) => current.Where(t => next.Projects.Contains(t)).ToList());

				if (!summaryResults.Any()) { return null; }

				IOrderedEnumerable<ProjectSummary> results;
				results = summaryResults.OrderByDescending(s => s.LastUpdated);

				return results.ToList();
			}
		}

	}
}
