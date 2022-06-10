using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class ProjectSummaryTests
	{
		[Fact]
		public void ProjectSummaryAreEqual()
		{
			bool areEqual = ps1.Equals(psEqual);
			bool areNotEqualDate = ps1.Equals(psDate);
			bool areNotEqualUid = ps1.Equals(psUid);
			bool areNotEqualScreenshot = ps1.Equals(psScreenshot);

			Assert.True(areEqual);
			Assert.False(areNotEqualDate);
			Assert.False(areNotEqualUid);
			Assert.False(areNotEqualScreenshot);
		}

		[Fact]
		public void ProjectSummaryHasDefaultProfileScreenshot()
		{
			Assert.Equal(string.Empty, ps1.ProfileScreenshot);
			Assert.NotEqual("/img/default.jpg", psScreenshot.ProfileScreenshot);
		}


		ProjectSummary ps1 = new()
		{
			ProjectUid = "project-1",
			ProjectName = "Project 1",
			ShortDescription = "project 1 Description",
			ProfileScreenshot = "",
			LastUpdated = new DateTime()
		};

		ProjectSummary psEqual = new()
		{
			ProjectUid = "project-1",
			ProjectName = "Project 1",
			ShortDescription = "project 1 Description",
			ProfileScreenshot = "",
			LastUpdated = new DateTime()
		};

		ProjectSummary psDate = new()
		{
			ProjectUid = "project-1",
			ProjectName = "Project 1",
			ShortDescription = "project 1 Description",
			ProfileScreenshot = "",
			LastUpdated = new DateTime() + TimeSpan.FromSeconds(1)
		};
		ProjectSummary psUid = new()
		{
			ProjectUid = "Project-1",
			ProjectName = "Project 1",
			ShortDescription = "project 1 Description",
			ProfileScreenshot = "",
			LastUpdated = new DateTime()
		};

		ProjectSummary psScreenshot = new()
		{
			ProjectUid = "project-1",
			ProjectName = "Project 1",
			ShortDescription = "project 1 Description",
			ProfileScreenshot = "/img/notdefault.jpg",
			LastUpdated = new DateTime()
		};

	}
}
