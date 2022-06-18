using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class PortfolioServiceTestData
	{
		private static readonly string Index = nameof(Index);

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
						"eloquent-lee-blog", "cascade-terrace-landscaping",
						"southwest-shiner-league" },
					ProjectUpdate = new() {
						"honeybee-daycare" },
					QueryGroup = "A"

				}},
			{ "ASP.NET", new TagTest{
					ProjectCreate = new() {
						"eloquent-lee-blog", "cascade-terrace-landscaping" },
					ProjectUpdate = new() {
						"honeybee-daycare", "cascade-terrace-landscaping" },
					QueryGroup = "A"
				}},
			{ "Blazor", new TagTest{
					ProjectCreate = new() {
						"eloquent-lee-blog"},
					ProjectUpdate = new() {
						"honeybee-daycare", "cascade-terrace-landscaping",
						"eloquent-lee-blog" },
					QueryGroup = "A"
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
						"mr-mechanical-auto", "southwest-shiner-league",
						"cascade-terrace-landscaping", "eloquent-lee-blog" },
					QueryGroup = "B"
				}},
			{ "CosmosDB", new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"mr-mechanical-auto", "southwest-shiner-league",
						"cascade-terrace-landscaping"},
					QueryGroup = "B"
				}},
			{ "SQL", new TagTest{
					ProjectCreate = new() { },
					ProjectUpdate = new() {
						"mr-mechanical-auto", "southwest-shiner-league" },
					QueryGroup = "B"
				}},
		};

		private readonly static Dictionary<string, Project> projects = new()
		{
			{"eloquent-lee-blog", new Project
				{
					Uid = "eloquent-lee-blog",
					ProjectName = "Eloquent Lee Blogging",
					ShortDescription = "Orchard Core CMS for an independent blogger",
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
			{ "honeybee-daycare", new Project
				{
					Uid = "honeybee-daycare",
					ProjectName = "HoneyBee Daycare",
					ShortDescription = "Blazor Server app for local daycare",
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
			int d = 0;
			foreach (string project in projects.Keys)
			{
				projects[project].ProjectName += " 2.0";
				projects[project].ShortDescription += " - Revised";
				projects[project].Summary = "<a href='http://www.zombieipsum.com/' target='_blank'>Zombie Ipsum</a>Zombie ipsum brains reversus ab cerebellum viral inferno, brein nam rick mend grimes malum cerveau cerebro.";
				projects[project].LastUpdated = new DateTime() + new TimeSpan(d, 0, 0, 0);
				d++;
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
						.OrderBy(p => p)
						.Select(p => new ProjectSummary(projects[p]))
						.ToList();
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
				if (tags[tagName].ProjectUpdate.Any() &&
					!tags[tagName].ProjectUpdate.All(p => !p.Equals(projects.Last().Value.Uid)))
				{
					List<ProjectSummary> projectSummaries = tags[tagName].ProjectUpdate
						.Where(p => !p.Equals(projects.Last().Value.Uid))
						.OrderBy(p => p)
						.Select(p => new ProjectSummary(projects[p]))
						.ToList();
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
				if (tags[tagName].ProjectUpdate.All(p => p.Equals(projects.Last().Value.Uid)))
				{
					deleteTagTests.Add(new object[] { tagName });
				}
			}
			return deleteTagTests;
		}

		public static IEnumerable<object[]> IndexTagUpdatedData()
		{
			List<ProjectSummary> summaries = new();
			foreach (string project in projects.Keys.SkipLast(1))
			{
				summaries.Add(new ProjectSummary(projects[project]));
			}
			List<ProjectSummary> projectSummaries =
				summaries.OrderByDescending(p => p.LastUpdated).ToList();
			return new List<object[]> { new object[] { nameof(Index), projectSummaries } };
		}

		public static IEnumerable<object[]> QueryProjectsByTagResultsData()
		{
			List<object[]> queryTests = new();

			IEnumerable<string>? groupA = tags.Keys.Where((t) => tags[t].QueryGroup.Equals("A"));
			IEnumerable<string>? groupB = tags.Keys.Where((t) => tags[t].QueryGroup.Equals("B"));

			queryTests.AddRange(CreateQueries(groupA));
			queryTests.AddRange(CreateQueries(groupB));

			return queryTests;
		}

		public static IEnumerable<object[]> QueryProjectsByTagNoResultsData()
		{
			List<string>? allValidTags = tags.Keys.Where((t) => tags[t].QueryGroup.Any()).ToList();
			return new List<object[]> { new object[] { allValidTags } };
		}

		static List<object[]> CreateQueries(IEnumerable<string> queryGroup)
		{
			List<object[]> queryTests = new();
			for (int i = queryGroup.Count(); i > 0; i--)
			{
				IEnumerable<string>? queryTags = queryGroup.TakeLast(i);
				IEnumerable<string>? seed =
					tags[queryTags.First()].ProjectUpdate
						.Where(p => !p.Equals(projects.Last().Value.Uid));

				IEnumerable<string>? results =
					queryTags.Aggregate(seed, (current, next) =>
						current.Where(t => tags[next].ProjectUpdate.Contains(t)));

				List<ProjectSummary> projectSummaries = new();
				if (results.Any())
				{
					foreach (string? result in results)
					{
						projectSummaries.Add(new ProjectSummary(projects[result]));
					}
				}

				queryTests.Add(new object[]
					{ queryTags.ToList(), projectSummaries.OrderByDescending(s => s.LastUpdated).ToList() });
			}

			return queryTests;
		}

	}
}
