using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class ProjectSummaryTestData
	{
		readonly static Project project = new()
		{
			Uid = "eloquent-lee-blog",
			ProjectName = "Eloquent Lee Blogging",
			ShortDescription = "Orchard Core CMS for an independent blogger",
			ProfileScreenshot = "image.jpg",
			LastUpdated = new DateTime()
		};

		readonly static ProjectSummary projectSummary = new()
		{
			ProjectUid = "eloquent-lee-blog",
			ProjectName = "Eloquent Lee Blogging",
			ShortDescription = "Orchard Core CMS for an independent blogger",
			ProfileScreenshot = "image.jpg",
			LastUpdated = new DateTime()
		};


		public static IEnumerable<object[]> SummaryEqualsData()
		{
			return new List<object[]>()
			{
				new object[] { projectSummary, projectSummary },
				new object[] { projectSummary, new ProjectSummary(project) },
				new object[] { new ProjectSummary(project), new ProjectSummary(project) }
			};
		}

		public static IEnumerable<object[]> SummaryNotEqualsData()
		{
			ProjectSummary modifedProjectSummary = new()
			{
				ProjectUid = projectSummary.ProjectUid,
				ProjectName = projectSummary.ProjectName.ToLower(),
				ShortDescription = projectSummary.ShortDescription,
				ProfileScreenshot = projectSummary.ProfileScreenshot,
				LastUpdated = new DateTime()
			};
			Project modifedProject = new()
			{
				Uid = projectSummary.ProjectUid.ToUpper(),
				ProjectName = projectSummary.ProjectName,
				ShortDescription = projectSummary.ShortDescription,
				ProfileScreenshot = projectSummary.ProfileScreenshot,
				LastUpdated = new DateTime()
			};

			return new List<object[]>()
			{
				new object[] { projectSummary, modifedProjectSummary },
				new object[] { projectSummary, new ProjectSummary(modifedProject) },
				new object[] { new ProjectSummary(project), new ProjectSummary(modifedProject) }
			};
		}

	}
}
