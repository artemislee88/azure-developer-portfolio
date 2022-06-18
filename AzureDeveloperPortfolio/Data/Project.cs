namespace AzureDeveloperPortfolio.Data
{
	public class Project
	{
		public Project()
		{

		}

		public string Uid { get; set; } = string.Empty;
		public string ProjectName { get; set; } = string.Empty;
		public string ShortDescription { get; set; } = string.Empty;
		public DateTime LastUpdated { get; set; }
		public string Summary { get; set; } = string.Empty;
		public string Markdown { get; set; } = string.Empty;
		public List<string> Screenshots { get; set; } = new();
		public string ProfileScreenshot { get; set; } = string.Empty;
		public string RepositoryURL { get; set; } = string.Empty;
		public string SiteURL { get; set; } = string.Empty;
		public bool IsDeployed { get; set; } = false;
		public List<string> Hosting { get; set; } = new();
		public List<string> Tags { get; set; } = new();
		public bool Published { get; set; } = false;
		public bool Featured { get; set; } = false;
		public List<string> RepositoryEvents { get; set; } = new();
		public List<string> DeploymentEvents { get; set; } = new();
		public string ETag { get; set; } = string.Empty;

		public override string ToString() =>
			$"{Uid} | {ProjectName} - Last Updated {LastUpdated}";

	}
}
