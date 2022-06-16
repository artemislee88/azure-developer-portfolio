using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Tests
{
	public class ProjectSummaryTests
	{
		[Theory(DisplayName = "EqualOverideTest_True")]
		[MemberData(nameof(ProjectSummaryTestData.SummaryEqualsData), MemberType = typeof(ProjectSummaryTestData))]
		public void ProjectSummaryEqualOverideTest_True(ProjectSummary ps1, ProjectSummary ps2)
		{
			bool areEqual = ps1.Equals(ps2);
			Assert.True(areEqual);
		}

		[Theory(DisplayName = "EqualOverideTest_False")]
		[MemberData(nameof(ProjectSummaryTestData.SummaryNotEqualsData), MemberType = typeof(ProjectSummaryTestData))]
		public void ProjectSummaryEqualOverideTest_False(ProjectSummary ps1, ProjectSummary ps2)
		{
			bool areNotEqual = ps1.Equals(ps2);
			Assert.False(areNotEqual);
		}
	}
}
