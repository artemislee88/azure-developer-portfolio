using AzureDeveloperPortfolio.Services;
using Microsoft.EntityFrameworkCore;

namespace AzureDeveloperPortfolio.Data
{
	public class DBInitializer
	{
		public async static void Initialize(IDbContextFactory<PortfolioContext> contextFactory)
		{
			PortfolioService service = new(contextFactory);
			foreach (Project? project in projects)
			{
				await service.CreateProjectAsync(project);
			}
		}

		static readonly List<Project> projects = new()
		{
			{ new Project() {
				Uid = "eloquent-lee-blog",
				ProjectName = "Eloquent Lee Blogging",
				ShortDescription = "Orchard Core CMS for an independent blogger",
				LastUpdated = DateTime.Now.AddDays(-1),
				Summary = "<a href='http://www.zombieipsum.com/' target='_blank'>Zombie Ipsum</a>Zombie ipsum brains reversus ab cerebellum viral inferno, brein nam rick mend grimes malum cerveau cerebro. De carne cerebro lumbering animata cervello corpora quaeritis. Summus thalamus brains sit​​, morbo basal ganglia vel maleficia? De braaaiiiins apocalypsi gorger omero prefrontal cortex undead survivor fornix dictum mauris.\nHi brains mindless mortuis limbic cortex soulless creaturas optic nerve, imo evil braaiinns stalking monstra hypothalamus adventus resi hippocampus dentevil vultus brain comedat cerebella pitiutary gland viventium.\nQui optic gland animated corpse, brains cricket bat substantia nigra max brucks spinal cord terribilem incessu brains zomby. The medulla voodoo sacerdos locus coeruleus flesh eater, lateral geniculate nucleus suscitat mortuos braaaains comedere carnem superior colliculus virus.",
				Tags = new List<string> { "Orchard Core", "NET Core", "ASP.NET", "Docker" },
				Published = true,
				Featured = true,
			}},
			{ new Project() {
				Uid = "cascade-terrace-landscaping",
				ProjectName = "Cascade Terrace Landscaping",
				ShortDescription = "ASP.NET Core Web app for small business",
				LastUpdated = DateTime.Now.AddDays(-2),
				Summary = "<a href='https://www.bobrosslipsum.com/' target='_blank'>Bob Ross Lipsum</a>\nI want everbody to be happy. That's what it's all about. It's so important to do something every day that will make you happy. Don't hurry. Take your time and enjoy. See there, told you that would be easy. It's hard to see things when you're too close. Take a step back and look.\nMaybe there was an old trapper that lived out here and maybe one day he went to check his beaver traps, and maybe he fell into the river and drowned. Making all those little fluffies that live in the clouds. I will take some magic white, and a little bit of Vandyke brown and a little touch of yellow. This is your world.\nWe want to use a lot pressure while using no pressure at all. If we're going to have animals around we all have to be concerned about them and take care of them.",
				Screenshots = new List<string> {
					"cascade-terrace-landscaping-home.png",
					"cascade-terrace-landscaping-services.png",
					"cascade-terrace-landscaping-contactus.png"
				},
				ProfileScreenshot = "cascade-terrace-landscaping-home.png",
				RepositoryURL = "https://github.com/artemislee88/CascadeTerraceLandscaping",
				Tags = new List<string> { "NET Core 6.0", "ASP.NET", "Docker" },
				Published = true,
				Featured = true,
			}},
			{ new Project() {
				Uid = "southwest-shiner-league",
				ProjectName = "SouthWest Shiner League",
				ShortDescription =  "ASP.NET Core Web app for community basketball league",
				LastUpdated = DateTime.Now.AddDays(-3),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Web App", "Azure CosmosDB", "Azure DevOps" },
				Tags = new List<string> { "Blazor", "SignalR", "NET Core 6.0", "ASP.NET", "Docker", "EF Core", "SQL" },
				Published = true
			}},
			{ new Project(){
				Uid = "honeybee-sunshine-daycare",
				ProjectName = "HoneyBee Sunshine Daycare",
				ShortDescription = "Blazor Server App for local daycare",
				LastUpdated = DateTime.Now.AddDays(-4),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Web App", "Azure CosmosDB", "Azure DevOps" },
				Tags = new List<string> { "Blazor", "SignalR", "NET Core 6.0","Docker", "EF Core", "SQL" },
				Published = true
			}},
			{ new Project() {
				Uid = "goodlife-photography",
				ProjectName = "Good Life Photography",
				ShortDescription = "NodeJS/ReactJS Photographer Portfolio",
				LastUpdated = DateTime.Now.AddDays(-5),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Static Apps", "Azure Storage", "Azure CosmosDB", "Azure DevOps" },
				Tags = new List <string> { "NodeJS", "ReactJS", "CosmosDB", "MongoDB" },
				Published = true
			}},
			{ new Project() {
				Uid = "cutter-the-barber",
				ProjectName = "Cutter The Barber",
				ShortDescription = "NodeJS/ReactJS appointment scheduling app for local barber",
				LastUpdated = DateTime.Now.AddDays(-6),
				Summary = "<a href='http://www.zombieipsum.com/' target='_blank'>Zombie Ipsum</a>Zombie ipsum brains reversus ab cerebellum viral inferno, brein nam rick mend grimes malum cerveau cerebro. De carne cerebro lumbering animata cervello corpora quaeritis. Summus thalamus brains sit​​, morbo basal ganglia vel maleficia? De braaaiiiins apocalypsi gorger omero prefrontal cortex undead survivor fornix dictum mauris.\nHi brains mindless mortuis limbic cortex soulless creaturas optic nerve, imo evil braaiinns stalking monstra hypothalamus adventus resi hippocampus dentevil vultus brain comedat cerebella pitiutary gland viventium.\nQui optic gland animated corpse, brains cricket bat substantia nigra max brucks spinal cord terribilem incessu brains zomby. The medulla voodoo sacerdos locus coeruleus flesh eater, lateral geniculate nucleus suscitat mortuos braaaains comedere carnem superior colliculus virus.",
				Hosting = new List<string> { "Azure Web App", "Azure Functions" },
				Tags = new List<string> { "ReactJS", "NodeJS", "JavaScript", "Docker"},
				Published = true
			}},
			{ new Project() {
				Uid = "swats-community-manager",
				ProjectName = "SouthWest Atlantis Community Manager",
				ShortDescription = "Python Django web app for local homeowners association",
				LastUpdated = DateTime.Now.AddDays(-7),
				Summary = "<a href='https://www.bobrosslipsum.com/' target='_blank'>Bob Ross Lipsum</a>\nI want everbody to be happy. That's what it's all about. It's so important to do something every day that will make you happy. Don't hurry. Take your time and enjoy. See there, told you that would be easy. It's hard to see things when you're too close. Take a step back and look.\nMaybe there was an old trapper that lived out here and maybe one day he went to check his beaver traps, and maybe he fell into the river and drowned. Making all those little fluffies that live in the clouds. I will take some magic white, and a little bit of Vandyke brown and a little touch of yellow. This is your world.\nWe want to use a lot pressure while using no pressure at all. If we're going to have animals around we all have to be concerned about them and take care of them.",
				Hosting = new List<string> {  "Azure Container App", "Azure CosmosDB" },
				Tags = new List<string> { "Python", "Django", "MongoDB", "ReactJS" },
				Published = true,
				Featured = true,
			}},
			{ new Project() {
				Uid = "watkins-family-reunion",
				ProjectName = "Watkins Family Reunion",
				ShortDescription = "Blazor WebAssembly app for family reunion",
				LastUpdated = DateTime.Now.AddDays(-8),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Web App", "Azure SQL Server", "Azure DevOps" },
				Tags = new List<string> { "Blazor", "SignalR", "NET Core 6.0" },
				Published = true
			}},
			{ new Project(){
				Uid = "know-your-morse-code",
				ProjectName = "Know Your Morse Code",
				ShortDescription = "Blazor WebAssembly morse code game",
				LastUpdated = DateTime.Now.AddDays(-9),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Web App", "Azure CosmosDB", "Azure DevOps" },
				Tags = new List<string> { "Blazor", "NET Core 6.0", "Docker" },
				Published = true
			}},
			{ new Project() {
				Uid = "roll-thunder-camp",
				ProjectName = "Roll Thunder Summer Camp",
				ShortDescription = "ASP.NET Core Web app for summer camp",
				LastUpdated = DateTime.Now.AddDays(-10),
				Summary = "<a href='https://vole.wtf/text-generator/' target='_blank'>vole.wtf - Text Generator</a>\nMutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best. So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.\nThere’s a voice that keeps on calling me. Down the road, that’s where I’ll always be. Every stop I make, I make a new friend. Can’t stay for long, just turn around and I’m gone again. Maybe tomorrow, I’ll want to settle down, Until tomorrow, I’ll just keep moving on.\nKnight Rider,a shadowy flight into the dangerous world of a man who does not exist.Michael Knight,a young loner on a crusade to champion the cause of the innocent, the helpless in a world of criminals who operate above the law.",
				Hosting = new List<string> { "Azure Static Apps", "Azure Storage", "Azure CosmosDB", "Azure DevOps" },
				Tags = new List<string> { "NodeJS", "ReactJS", "CosmosDB", "MongoDB", "JavaScript" },
				Published = true
			}}
		};
	}
}

