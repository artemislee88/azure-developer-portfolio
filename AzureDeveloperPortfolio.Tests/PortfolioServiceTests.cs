using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;

namespace AzureDeveloperPortfolio.Tests
{
	public class PortfolioServiceTests : IClassFixture<TestDbContextFactory>
	{
		public PortfolioServiceTests(TestDbContextFactory contextFactory) => ContextFactory = contextFactory;

		public TestDbContextFactory ContextFactory { get; }

		[Fact]
		public void CreateProjectAsync()
		{
			PortfolioService? service = new(ContextFactory);
			string eloquentUid = service.CreateProjectAsync(eloquent).Result;
			string cascadeUid = service.CreateProjectAsync(cascade).Result;
			string shinerUid = service.CreateProjectAsync(shiner).Result;
			Assert.Equal("eloquent-lee-blog", eloquentUid);

			PortfolioContext context = ContextFactory.CreateDbContext();

			Tag? orchardcore = context.Tags.Where(t => t.TagName == "Orchard Core").FirstOrDefault();
			Assert.NotNull(orchardcore);
			Assert.All(orchardcore?.Projects.AsEnumerable(), item => Assert.Contains("eloquent-lee-blog", item.ProjectUid));

			Tag? netcore6 = context.Tags.Where(t => t.TagName == "NET Core 6.0").FirstOrDefault();
			Assert.NotNull(netcore6);
			Assert.Collection(netcore6?.Projects.AsEnumerable(),
				item => Assert.Contains("eloquent-lee-blog", item.ProjectUid),
				item => Assert.Contains("cascade-terrace-landscaping", item.ProjectUid),
				item => Assert.Contains("southwest-shiner-league", item.ProjectUid)
				);

			Tag? aspNet = context.Tags.Where(t => t.TagName == "ASP.NET").FirstOrDefault();
			Assert.NotNull(aspNet);
			Assert.Collection(aspNet?.Projects.AsEnumerable(),
				item => Assert.Contains("eloquent-lee-blog", item.ProjectUid),
				item => Assert.Contains("cascade-terrace-landscaping", item.ProjectUid),
				item => Assert.Contains("southwest-shiner-league", item.ProjectUid)
				);

			Tag? docker = context.Tags.Where(t => t.TagName == "Docker").FirstOrDefault();
			Assert.NotNull(docker);
			Assert.Collection(docker?.Projects.AsEnumerable(),
				item => Assert.Contains("eloquent-lee-blog", item.ProjectUid),
				item => Assert.Contains("cascade-terrace-landscaping", item.ProjectUid),
				item => Assert.Contains("southwest-shiner-league", item.ProjectUid)
				);

			Tag? blazor = context.Tags.Where(t => t.TagName == "Blazor").FirstOrDefault();
			Assert.NotNull(blazor);
			Assert.All(blazor?.Projects.AsEnumerable(), item => Assert.Contains("southwest-shiner-league", item.ProjectUid));

			Tag? signalR = context.Tags.Where(t => t.TagName == "SignalR").FirstOrDefault();
			Assert.NotNull(signalR);
			Assert.All(signalR?.Projects.AsEnumerable(), item => Assert.Contains("southwest-shiner-league", item.ProjectUid));

			Tag? index = context.Tags.Where(t => t.TagName == "index").FirstOrDefault();
			Assert.NotNull(index);
			Assert.Collection(index?.Projects.AsEnumerable(),
				item => Assert.Contains("eloquent-lee-blog", item.ProjectUid),
				item => Assert.Contains("cascade-terrace-landscaping", item.ProjectUid),
				item => Assert.Contains("southwest-shiner-league", item.ProjectUid)
				);

			ProjectSummary? indexProjectSummary = index?.Projects.Where(p => p.ProjectUid == "eloquent-lee-blog").FirstOrDefault();
			Assert.Equal(eloquent.Uid, indexProjectSummary?.ProjectUid);
			Assert.Equal(eloquent.ProjectName, indexProjectSummary?.ProjectName);
			Assert.Equal(eloquent.ShortDescription, indexProjectSummary?.ShortDescription);
			Assert.Equal(new DateTime(), indexProjectSummary?.LastUpdated);
			Assert.Equal("/img/default.jpg", indexProjectSummary?.ProfileScreenshot);

		}


		Project eloquent = new()
		{
			Uid = "eloquent-lee-blog",
			ProjectName = "Eloquent Lee Blogging",
			ShortDescription = "Orchard Core CMS for an independent blogger",
			Summary = "",
			Tags = new List<string> {
				"Orchard Core",
				"NET Core 6.0",
				"ASP.NET",
				"Docker"
			},
			Published = true,
			Featured = true,
		};

		Project cascade = new()
		{
			Uid = "cascade-terrace-landscaping",
			ProjectName = "Cascade Terrace Landscaping",
			ShortDescription = "ASP.NET Core Web app for small business",
			Summary = "",
			Markdown = "",
			Screenshots = new List<string> {
					"cascade-terrace-landscaping-home.png",
					"cascade-terrace-landscaping-services.png",
					"cascade-terrace-landscaping-contactus.png"
				},
			ProfileScreenshot = "cascade-terrace-landscaping-home.png",
			RepositoryURL = "https://github.com/artemislee88/CascadeTerraceLandscaping",
			Tags = new List<string> {
					"NET Core 6.0",
					"ASP.NET",
					"EF Core 6.0",
					"Docker"
			},
			Published = true,
			Featured = true,
		};

		Project shiner = new()
		{
			Uid = "southwest-shiner-league",
			ProjectName = "SouthWest Shiner League",
			ShortDescription = "ASP.NET Core Web app for community league",
			Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
			Tags = new List<string> {
				"Blazor",
				"SignalR",
				"NET Core 6.0",
				"ASP.NET",
				"Docker"
			},
		};

	}
}