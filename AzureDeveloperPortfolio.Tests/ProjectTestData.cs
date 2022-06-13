using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class ProjectTestData
	{
		readonly static string[] tags =
			{ "Docker", "Orchard Core", "NET Core 6.0" , "ASP.NET", "EF Core 6.0" , "Blazor" , "SignalR"};
		readonly static List<Project> projectList = new() {
			new Project
			{
				Uid = "eloquent-lee-blog",
				ProjectName = "Eloquent Lee Blogging",
				ShortDescription = "Orchard Core CMS for an independent blogger",
				Tags = new List<string> { tags[0], tags[1] }
			},
			new Project
			{
				Uid = "cascade-terrace-landscaping",
				ProjectName = "Cascade Terrace Landscaping",
				ShortDescription = "ASP.NET Core Web app for small business",
				ProfileScreenshot = "cascade-terrace-landscaping-home.png",
				Tags = new List<string> {tags[0], tags[2], tags[3] }
			},
			new Project
			{
				Uid = "southwest-shiner-league",
				ProjectName = "SouthWest Shiner League",
				ShortDescription = "ASP.NET Core Web app for community league",
				Tags = new List<string> { tags[0], tags[4], tags[5], tags[6] }
			}
		};

		public static IEnumerable<object[]> ProjectData()
		{
			List<object[]> createProjectTests = new();
			foreach (Project project in projectList)
			{
				createProjectTests.Add(new object[] { project, project.Uid });
			}
			return createProjectTests;
		}

		public static IEnumerable<object[]> TagData()
		{
			List<object[]> createTagTests = new();

			foreach (string tag in tags)
			{
				List<ProjectSummary> projectSummaries = new();
				foreach (Project project in projectList)
				{
					if (project.Tags.Contains(tag))
					{
						projectSummaries.Add(new ProjectSummary(project));
					}
				}
				createTagTests.Add(new object[] { tag, projectSummaries });
			}
			return createTagTests;
		}

		public static IEnumerable<object[]> IndexData()
		{
			List<object[]> createIndexTagTest = new();
			List<ProjectSummary> projectSummaries = new();
			foreach (Project project in projectList)
			{
				projectSummaries.Add(new ProjectSummary(project));
			}
			createIndexTagTest.Add(new object[] { "index", projectSummaries });

			return createIndexTagTest;
		}

		public static IEnumerable<object[]> NoProfileScreenshotData()
		{
			List<object[]> createIndexTagTest = new();
			foreach (Project project in projectList)
			{
				if (project.ProfileScreenshot is null)
				{
					createIndexTagTest.Add(new object[] { project.Uid });
				}
			}
			return createIndexTagTest;
		}

	}
}
