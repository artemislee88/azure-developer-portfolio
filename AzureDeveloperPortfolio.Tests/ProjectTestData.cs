using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class ProjectTestData
	{
		private static readonly string Index = nameof(Index);
		private static readonly string Featured = nameof(Featured);

		private readonly static Dictionary<string, TagTest> tags = new()
		{
			{ "Docker", new TagTest () {
					ProjectCreate = new() {
						"eloquent-lee-blog", "cascade-terrace-landscaping",
						"southwest-shiner-league", "mr-mechanical-auto" },
					ProjectUpdate = new() { }
				}},
			{ "Orchard Core", new TagTest{
					ProjectCreate = new() {
						"eloquent-lee-blog", "cascade-terrace-landscaping"},
					ProjectUpdate = new() {
						"eloquent-lee-blog", "mr-mechanical-auto" }
				}},
			{ "ASP.NET", new TagTest{
					ProjectCreate = new() {
						"eloquent-lee-blog", "cascade-terrace-landscaping", "southwest-shiner-league" },
					ProjectUpdate = new() {
						"eloquent-lee-blog", "cascade-terrace-landscaping"}
				}},
			{ "Blazor", new TagTest{
					ProjectCreate = new() {
						"cascade-terrace-landscaping", "southwest-shiner-league" },
					ProjectUpdate = new() {
						"southwest-shiner-league" }
				}},
			{ "SignalR", new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"mr-mechanical-auto" }
				}},
			{ "EF Core", new TagTest{
					ProjectCreate = new() {
						"eloquent-lee-blog", "mr-mechanical-auto" },
					ProjectUpdate = new() {
						"cascade-terrace-landscaping", "southwest-shiner-league" }
				}},
			{ "CosmosDB", new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"southwest-shiner-league", "mr-mechanical-auto"}
				}},
			{ "SQL", new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"cascade-terrace-landscaping", "mr-mechanical-auto" }
				}},
			{ nameof(Featured), new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"mr-mechanical-auto"}
				}},
		};

		private readonly static Dictionary<string, Project> projects = new()
		{
			{"eloquent-lee-blog", new Project
				{
					Uid = "eloquent-lee-blog",
					ProjectName = "Eloquent Lee Blogging",
					ShortDescription = "Orchard Core CMS for an independent blogger",
					ProfileScreenshot = "cascade-terrace-landscaping-home.png",
					Tags = new List<string>()
				}
			},
			{"cascade-terrace-landscaping", new Project
				{
					Uid = "cascade-terrace-landscaping",
					ProjectName = "Cascade Terrace Landscaping",
					ShortDescription = "ASP.NET Core Web app for small business",
					ProfileScreenshot = "cascade-terrace-landscaping-home.png",
					Tags = new List<string>()
				}
			},
			{ "southwest-shiner-league", new Project
				{
					Uid = "southwest-shiner-league",
					ProjectName = "SouthWest Shiner League",
					ShortDescription = "Blazor WebAssembly app for community league",
					Tags = new List<string>()
				}
			},
			{ "mr-mechanical-auto", new Project
				{
					Uid = "mr-mechanical-auto",
					ProjectName = "Mr Mechanical Auto Repairs",
					ShortDescription = "Blazor Server app for auto repair shop",
					Tags = new List<string>()
				}
			}

		};

		public static IEnumerable<object[]> CreateProjectData()
		{
			List<object[]> createProjectTests = new();

			foreach (string project in projects.Keys)
			{
				foreach (string tagName in tags.Keys)
				{
					if (tags[tagName].ProjectCreate.Contains(project))
					{
						projects[project].Tags.Add(tagName);
					}
				}
				createProjectTests.Add(new object[] { projects[project], projects[project].Uid });
			}
			return createProjectTests;
		}

		public static IEnumerable<object[]> CreateProjectTagsCreatedData()
		{
			List<object[]> createTagTests = new();
			foreach (string tagName in tags.Keys)
			{
				if (tags[tagName].ProjectCreate.Any())
				{
					List<ProjectSummary> projectSummaries = tags[tagName].ProjectCreate
						.Select(p => new ProjectSummary(projects[p])).ToList();
					createTagTests.Add(new object[] { tagName, projectSummaries });
				}
			}
			return createTagTests;
		}

		public static IEnumerable<object[]> IndexTagCreatedData()
		{
			List<ProjectSummary> projectSummaries = new();
			foreach (string project in projects.Keys)
			{
				projectSummaries.Add(new ProjectSummary(projects[project]));
			}
			return new List<object[]> { new object[] { nameof(Index), projectSummaries } };
		}

		public static IEnumerable<object[]> GetProjectData()
		{
			List<object[]> createProjectTests = new();

			foreach (string project in projects.Keys)
			{
				createProjectTests.Add(new object[] { projects[project], projects[project].Uid });
			}
			return createProjectTests;
		}

		public static IEnumerable<object[]> UpdateProjectData()
		{
			List<object[]> updateProjectTests = new();
			foreach (string project in projects.Keys)
			{
				projects[project].ProjectName += " - UPDATED";
				projects[project].ShortDescription += " - UPDATED";
				projects[project].Summary = "<a href='http://www.zombieipsum.com/' target='_blank'>Zombie Ipsum</a>Zombie ipsum brains reversus ab cerebellum viral inferno, brein nam rick mend grimes malum cerveau cerebro.";
				projects[project].Tags.Clear();
				foreach (string tagName in tags.Keys)
				{
					if (tags[tagName].ProjectUpdate.Contains(project))
					{
						projects[project].Tags.Add(tagName);
					}
				}
				updateProjectTests.Add(new object[] { projects[project] });
			}
			return updateProjectTests;
		}

		public static IEnumerable<object[]> UpdateProjectTagsUpdatedData()
		{
			List<object[]> updateTagTests = new();
			foreach (string tagName in tags.Keys)
			{
				if (tags[tagName].ProjectUpdate.Any())
				{
					List<ProjectSummary> projectSummaries = tags[tagName].ProjectUpdate
						.Select(p => new ProjectSummary(projects[p])).ToList();
					updateTagTests.Add(new object[] { tagName, projectSummaries });
				}
			}
			return updateTagTests;
		}

		public static IEnumerable<object[]> UpdateProjectTagsDeletedData()
		{
			List<object[]> deletedTagTests = new();
			foreach (string tagName in tags.Keys)
			{
				if (!tags[tagName].ProjectUpdate.Any())
				{
					deletedTagTests.Add(new object[] { tagName });
				}
			}
			return deletedTagTests;
		}

		public static IEnumerable<object[]> DeleteProjectData()
		{
			return new List<object[]> { new object[] { projects.Last().Value.Uid } };
		}


		public static IEnumerable<object[]> DeleteProjectTagsUpdatedData()
		{
			List<object[]> updateTagTests = new();
			foreach (string tagName in tags.Keys)
			{
				if (tags[tagName].ProjectUpdate.Any() && !tags[tagName].ProjectUpdate.All(p => !p.Equals(projects.Last().Value.Uid)))
				{
					List<ProjectSummary> projectSummaries = tags[tagName].ProjectUpdate
						.Where(p => !p.Equals(projects.Last().Value.Uid))
						.Select(p => new ProjectSummary(projects[p])).ToList();
					if (projectSummaries.Any())
					{
						updateTagTests.Add(new object[] { tagName, projectSummaries });
					}
				}
			}
			return updateTagTests;
		}

		public static IEnumerable<object[]> DeleteProjectTagsDeletedData()
		{
			List<object[]> deleteTagTests = new();
			foreach (string tagName in tags.Keys)
			{
				if (!tagName.Equals(nameof(Featured)) && tags[tagName].ProjectUpdate.All(p => p.Equals(projects.Last().Value.Uid)))
				{
					deleteTagTests.Add(new object[] { tagName });
				}
			}
			return deleteTagTests;
		}

		public static IEnumerable<object[]> IndexTagUpdatedData()
		{
			List<ProjectSummary> projectSummaries = new();
			foreach (string project in projects.Keys.SkipLast(1))
			{
				projectSummaries.Add(new ProjectSummary(projects[project]));
			}
			return new List<object[]> { new object[] { nameof(Index), projectSummaries } };
		}

	}
}
