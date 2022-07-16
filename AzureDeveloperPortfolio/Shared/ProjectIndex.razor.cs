using AzureDeveloperPortfolio.Data;
using Microsoft.AspNetCore.Components;

namespace AzureDeveloperPortfolio.Shared
{
	public partial class ProjectIndex
	{
		//public Guid Guid = Guid.NewGuid();
		public string ModalDisplay = "none;";
		public string ModalClass = "";
		public bool ShowBackdrop = false;

		[Parameter, EditorRequired]
		public List<ProjectSummary>? Index { get; set; }

		public void Open()
		{
			ModalDisplay = "block;";
			ModalClass = "Show";
			ShowBackdrop = true;
			StateHasChanged();
		}

		public void Close()
		{
			ModalDisplay = "none";
			ModalClass = "";
			ShowBackdrop = false;
			StateHasChanged();
		}
	}
}