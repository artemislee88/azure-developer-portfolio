using AzureDeveloperPortfolio.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AzureDeveloperPortfolio.Pages
{
	public partial class ProjectDetail
	{
		[Parameter]
		public string? ProjectUid { get; set; }
		[Inject]
		protected IPortfolioService? PortfolioService { get; set; } 
		protected bool Loading { get; set; } = true;
		protected bool ShowProjectIndex { get; set; }
		protected Project? Project { get; set; }
		protected Tag? Index { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			try
			{
				if (PortfolioService is not null && ProjectUid is not null)
				{
					Project = await PortfolioService.GetProjectAsync(ProjectUid);
					Loading = false;
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
			}
			// return base.OnParametersSetAsync();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				if (PortfolioService is not null)
				{
					Index = await PortfolioService.GetTagAsync("Index");
				}
			}
		}

		private void ToggleProjectList(MouseEventArgs e)
		{
			ShowProjectIndex = !ShowProjectIndex;
		}


	}

}
