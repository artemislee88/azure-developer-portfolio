using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;
using AzureDeveloperPortfolio.Tests.Attributes;

namespace AzureDeveloperPortfolio.Tests
{
	[TestCaseOrderer("AzureDeveloperPortfolio.Tests.Orderers.PriorityOrderer", "AzureDeveloperPortfolio.Tests")]
	public class PortfolioServiceTests : IClassFixture<TestDbContextFactory>
	{
		private static readonly string Featured = nameof(Featured);
		public TestDbContextFactory ContextFactory { get; }
		public PortfolioServiceTests(TestDbContextFactory contextFactory) => ContextFactory = contextFactory;



		[Theory(DisplayName = "CreateProject"), TestPriority(00100)]
		[MemberData(nameof(PortfolioServiceTestData.CreateProjectData), MemberType = typeof(PortfolioServiceTestData))]
		public void CreateProjectAsyncTest_ProjectIsCreated(Project project, string expectedUid)
		{
			PortfolioService service = new(ContextFactory);
			string actualUid = service.CreateProjectAsync(project).Result;

			Assert.IsType<Project>(project);
			Assert.Equal(expectedUid, actualUid);
		}


		[Theory(DisplayName = "GetTag_Found - CreateProject.Tags_Created"), TestPriority(00102)]
		[MemberData(nameof(PortfolioServiceTestData.CreateProjectTagsCreatedData), MemberType = typeof(PortfolioServiceTestData))]
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
		[MemberData(nameof(PortfolioServiceTestData.IndexTagCreatedData), MemberType = typeof(PortfolioServiceTestData))]
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
		[MemberData(nameof(PortfolioServiceTestData.GetProjectData), MemberType = typeof(PortfolioServiceTestData))]
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
		[MemberData(nameof(PortfolioServiceTestData.UpdateProjectData), MemberType = typeof(PortfolioServiceTestData))]
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
		[MemberData(nameof(PortfolioServiceTestData.UpdateProjectTagsUpdatedData), MemberType = typeof(PortfolioServiceTestData))]
		public void UpdateProjectAsyncTest_TagsUpdated(string tagName, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(tagName).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects.OrderBy(s => s.ProjectUid);

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "UpdateProject_EmptyTags_Deleted"), TestPriority(00302)]
		[MemberData(nameof(PortfolioServiceTestData.UpdateProjectTagsDeletedData), MemberType = typeof(PortfolioServiceTestData))]
		public void UpdateProjectAsyncTest_TagDeleted(string deletedTagName)
		{
			PortfolioService service = new(ContextFactory);
			Task<Tag?>? nullTask = service.GetTagAsync(deletedTagName);

			Assert.Null(nullTask.Result);
		}

		[Theory(DisplayName = "DeleteProject"), TestPriority(00400)]
		[MemberData(nameof(PortfolioServiceTestData.DeleteProjectData), MemberType = typeof(PortfolioServiceTestData))]
		public void DeleteProjectAsyncTest(string projectUid)
		{
			PortfolioService service = new(ContextFactory);
			Task? task = service.DeleteProjectAsync(projectUid);
			task.Wait();

			Assert.True(task.IsCompletedSuccessfully);

			Task<Project?>? nullTask = service.GetProjectAsync(projectUid);

			Assert.Null(nullTask.Result);
		}

		[Theory(DisplayName = "DeleteProject.Tags_Updated"), TestPriority(00401)]
		[MemberData(nameof(PortfolioServiceTestData.DeleteProjectTagsUpdatedData), MemberType = typeof(PortfolioServiceTestData))]
		public void DeleteProjectAsyncTest_TagsUpdated(string tagName, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(tagName).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects.OrderBy(s => s.ProjectUid);

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "DeleteProject.Tags_Deleted"), TestPriority(00402)]
		[MemberData(nameof(PortfolioServiceTestData.DeleteProjectTagsDeletedData), MemberType = typeof(PortfolioServiceTestData))]
		public void DeleteProjectAsyncTest_TagsDeleted(string deletedTagName)
		{
			PortfolioService service = new(ContextFactory);
			Task<Tag?>? nullTask = service.GetTagAsync(deletedTagName);

			Assert.Null(nullTask.Result);
		}

		[Theory(DisplayName = "DeleteProject_IndexTag_Updated"), TestPriority(00403)]
		[MemberData(nameof(PortfolioServiceTestData.IndexTagUpdatedData), MemberType = typeof(PortfolioServiceTestData))]
		public void DeleteProjectAsyncTest_IndexTagUpdated(string index, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			Tag? actualTag = service.GetTagAsync(index).Result;
			IEnumerable<ProjectSummary>? actualSummaries = actualTag?.Projects;

			Assert.NotNull(actualTag);
			Assert.All(actualSummaries, item => Assert.IsType<ProjectSummary>(item));
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Fact(DisplayName = "DeleteProject_FeaturedTag_Exists"), TestPriority(00404)]
		public void DeleteProjectAsyncTest_FeaturedTagExists()
		{
			PortfolioService service = new(ContextFactory);
			Tag? featuredTag = service.GetTagAsync(nameof(Featured)).Result;

			Assert.NotNull(featuredTag);
		}

		[Theory(DisplayName = "QueryProjectsByTag_Results"), TestPriority(00500)]
		[MemberData(nameof(PortfolioServiceTestData.QueryProjectsByTagResultsData), MemberType = typeof(PortfolioServiceTestData))]
		public void QueryProjectsByTagAsyncTest_Results(List<string> tagNames, List<ProjectSummary> expectedSummaries)
		{
			PortfolioService service = new(ContextFactory);
			List<ProjectSummary>? actualSummaries = service.QueryProjectsByTagAsync(tagNames).Result;

			Assert.NotNull(actualSummaries);
			Assert.Equal(expectedSummaries, actualSummaries);
		}

		[Theory(DisplayName = "QueryProjectsByTag_NoResults"), TestPriority(00501)]
		[MemberData(nameof(PortfolioServiceTestData.QueryProjectsByTagNoResultsData), MemberType = typeof(PortfolioServiceTestData))]
		public void QueryProjectsByTagAsyncTest_NoResults(List<string> tagNames)
		{
			PortfolioService service = new(ContextFactory);
			Task<List<ProjectSummary>?>? nullTask = service.QueryProjectsByTagAsync(tagNames);

			Assert.Null(nullTask.Result);
		}
	}
}