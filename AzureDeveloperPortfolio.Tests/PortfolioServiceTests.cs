using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;
using AzureDeveloperPortfolio.Tests.Attributes;

namespace AzureDeveloperPortfolio.Tests
{
	[TestCaseOrderer("AzureDeveloperPortfolio.Tests.Orderers.PriorityOrderer", "AzureDeveloperPortfolio.Tests")]
	public class PortfolioServiceTests : IClassFixture<TestDbContextFactory>
	{
		public PortfolioServiceTests(TestDbContextFactory contextFactory) => ContextFactory = contextFactory;

		public TestDbContextFactory ContextFactory { get; }


		[Theory, TestPriority(00100)]
		[MemberData(nameof(ProjectTestData.ProjectData), MemberType = typeof(ProjectTestData))]
		public void CreateProjectAsyncTest_ProjectIsCreated(Project project, string expectedUid)
		{
			PortfolioService service = new(ContextFactory);
			string resultUid = service.CreateProjectAsync(project).Result;
			Assert.IsType<Project>(project);
			Assert.Equal(expectedUid, resultUid);
		}

		[Theory, TestPriority(00101)]
		[MemberData(nameof(ProjectTestData.TagData), MemberType = typeof(ProjectTestData))]
		public void CreateProjectAsyncTest_TagsAreCreated(string tagName, List<ProjectSummary> projectSummaries)
		{
			PortfolioContext context = ContextFactory.CreateDbContext();
			Tag? result = context.Tags.Where(t => t.TagName == tagName).FirstOrDefault();
			IEnumerable<ProjectSummary>? resultProjectSummaries = result?.Projects;
			Assert.NotNull(result);
			Assert.All(resultProjectSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(resultProjectSummaries, projectSummaries);
		}

		[Theory, TestPriority(00102)]
		[MemberData(nameof(ProjectTestData.IndexData), MemberType = typeof(ProjectTestData))]
		public void CreateProjectAsyncTest_IndexTagisCreated(string tagName, List<ProjectSummary> projectSummaries)
		{
			PortfolioContext context = ContextFactory.CreateDbContext();
			Tag? result = context.Tags.Where(t => t.TagName == tagName).FirstOrDefault();
			IEnumerable<ProjectSummary>? resultProjectSummaries = result?.Projects;
			Assert.NotNull(result);
			Assert.All(resultProjectSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(resultProjectSummaries, projectSummaries);
		}

		[Fact, TestPriority(00103)]
		public void ProjectSummaryHasDefaultLastUpdated()
		{
			PortfolioContext context = ContextFactory.CreateDbContext();
			Tag? index = context.Tags.Where(t => t.TagName == "index").FirstOrDefault();
			Assert.All(index?.Projects, item => Assert.Equal(new DateTime(), item.LastUpdated));
		}

	}
}