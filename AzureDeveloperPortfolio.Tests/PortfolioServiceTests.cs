using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;
using AzureDeveloperPortfolio.Tests.Attributes;

namespace AzureDeveloperPortfolio.Tests
{
	[TestCaseOrderer("AzureDeveloperPortfolio.Tests.Orderers.PriorityOrderer", "AzureDeveloperPortfolio.Tests")]
	public class PortfolioServiceTests : IClassFixture<TestDbContextFactory>
	{
		public TestDbContextFactory ContextFactory { get; }
		public PortfolioServiceTests(TestDbContextFactory contextFactory) => ContextFactory = contextFactory;



		[Theory(DisplayName = "CreateProject"), TestPriority(00100)]
		[MemberData(nameof(ProjectTestData.CreateProjectData), MemberType = typeof(ProjectTestData))]
		public void CreateProjectAsyncTest_ProjectIsCreated(Project project, string expectedUid)
		{
			PortfolioService service = new(ContextFactory);
			string actualUid = service.CreateProjectAsync(project).Result;

			Assert.IsType<Project>(project);
			Assert.Equal(expectedUid, actualUid);
		}


		[Theory(DisplayName = "GetTag_Found - CreateProject.Tags_Created"), TestPriority(00102)]
		[MemberData(nameof(ProjectTestData.TagsCreatedData), MemberType = typeof(ProjectTestData))]
		public void GetTagAsyncTest_ProjectCreate_TagsCreated(string tagName, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(tagName).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects;

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "GetTag_NotFound"), TestPriority(00103)]
		[InlineData("Invalid Tag")]
		public void GetTagAsyncTest_TagNotFound(string invalidTagName)
		{
			PortfolioService? service = new(ContextFactory);
			Task<Tag?>? nullTask = service.GetTagAsync(invalidTagName);

			Assert.Null(nullTask.Result);
		}

		[Theory(DisplayName = "CreateProject_IndexTag_Created"), TestPriority(00104)]
		[MemberData(nameof(ProjectTestData.IndexTagCreatedData), MemberType = typeof(ProjectTestData))]
		public void CreateProjectAsyncTest_IndexTagCreated(string index, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(index).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects;

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.All(actualTag?.Projects, item => Assert.Equal(new DateTime(), item.LastUpdated));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "GetProject_Found"), TestPriority(00200)]
		[MemberData(nameof(ProjectTestData.GetProjectData), MemberType = typeof(ProjectTestData))]
		public void GetProjectAsyncTest_ProjectFound(Project expectedProject, string projectUid)
		{
			PortfolioService? service = new(ContextFactory);
			Project? actualProject = service.GetProjectAsync(projectUid).Result;

			Assert.IsType<Project>(actualProject);
			Assert.Equal(expectedProject.Uid, actualProject?.Uid);
			Assert.Equal(expectedProject.ProjectName, actualProject?.ProjectName);
			Assert.Equal(expectedProject.ShortDescription, actualProject?.ShortDescription);
			Assert.Equal(expectedProject.Tags, actualProject?.Tags);
		}

		[Theory(DisplayName = "GetProject_NotFound"), TestPriority(00201)]
		[InlineData("Invalid Project")]
		public void GetProjectAsyncTest_ProjectNotFound(string invalidUid)
		{
			PortfolioService? service = new(ContextFactory);
			Task<Project?>? nullTask = service.GetProjectAsync(invalidUid);

			Assert.Null(nullTask.Result);
		}

		[Theory(DisplayName = "UpdateProject"), TestPriority(00300)]
		[MemberData(nameof(ProjectTestData.UpdateProjectData), MemberType = typeof(ProjectTestData))]
		public void UpdateProjectAsyncTest_ProjectUpdated(Project expectedProject)
		{
			PortfolioService service = new(ContextFactory);
			Task? task = service.UpdateProjectAsync(expectedProject);
			task.Wait();
			Assert.True(task.IsCompletedSuccessfully);

			Project? actualProject = service.GetProjectAsync(expectedProject.Uid).Result;

			Assert.IsType<Project>(actualProject);
			Assert.Equal(expectedProject.Uid, actualProject?.Uid);
			Assert.Equal(expectedProject.ProjectName, actualProject?.ProjectName);
			Assert.Equal(expectedProject.ShortDescription, actualProject?.ShortDescription);
			Assert.Equal(expectedProject.Summary, actualProject?.Summary);
			Assert.Equal(expectedProject.Markdown, actualProject?.Markdown);
			Assert.Equal(expectedProject.Screenshots, actualProject?.Screenshots);
			Assert.Equal(expectedProject.ProfileScreenshot, actualProject?.ProfileScreenshot);
			Assert.Equal(expectedProject.RepositoryURL, actualProject?.RepositoryURL);
			Assert.Equal(expectedProject.SiteURL, actualProject?.SiteURL);
			Assert.Equal(expectedProject.IsDeployed, actualProject?.IsDeployed);
			Assert.Equal(expectedProject.Hosting, actualProject?.Hosting);
			Assert.Equal(expectedProject.Tags, actualProject?.Tags);
			Assert.Equal(expectedProject.Published, actualProject?.Published);
			Assert.Equal(expectedProject.Featured, actualProject?.Featured);
		}

		[Theory(DisplayName = "UpdateProject.Tags_Updated"), TestPriority(00301)]
		[MemberData(nameof(ProjectTestData.TagsUpdatedData), MemberType = typeof(ProjectTestData))]
		public void UpdateProjectAsyncTest_TagsUdpdated(string tagName, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(tagName).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects;

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "UpdateProject_EmptyTags_Deleted"), TestPriority(00302)]
		[MemberData(nameof(ProjectTestData.UpdateTagsDeletedData), MemberType = typeof(ProjectTestData))]
		public void UpdateProjectAsyncTest_IndexTagUpdated(string deletedTagName)
		{
			PortfolioService service = new(ContextFactory);
			Task<Tag?>? nullTask = service.GetTagAsync(deletedTagName);

			Assert.Null(nullTask.Result);
		}
	}
}