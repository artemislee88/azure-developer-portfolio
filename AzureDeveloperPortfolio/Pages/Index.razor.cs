using AzureDeveloperPortfolio.Data;
using Microsoft.AspNetCore.Components;

namespace AzureDeveloperPortfolio.Pages
{
	public partial class Index
	{
		[Inject]
		protected IBlobStorageService? BlobStorageService { get; set; }
	}
}
