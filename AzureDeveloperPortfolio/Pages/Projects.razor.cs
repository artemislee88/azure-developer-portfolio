using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Shared;
using Microsoft.AspNetCore.Components;

namespace AzureDeveloperPortfolio.Pages
{
	public partial class Projects
	{
		[Inject]
		protected IPortfolioService? PortfolioService { get; set; }
		protected bool Loading { get; set; } = true;
		protected bool ShowProjectIndex { get; set; }
		protected List<Tag>? Tags { get; set; }
		protected List<Project>? Featured { get; set; }
		protected Tag? Index { get; set; }
		private ProjectIndex? ProjectIndex { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (PortfolioService is not null)
			{
				try
				{
					Featured = await PortfolioService.GetFeatureProjects();

					Loading = false;
				}
				catch (Exception ex)
				{

					Console.Error.WriteLine(ex);
				}
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				if (PortfolioService is not null)
				{
					Index = await PortfolioService.GetTagAsync("Index");
					Tags = await PortfolioService.GetTagsAsync();

				}
			}
		}


	}
}
