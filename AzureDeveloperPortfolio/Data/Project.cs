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
		public string? Summary { get; set; }
		public string? Markdown { get; set; }
		public List<string>? Screenshots { get; set; }
		public string ProfileScreenshot { get; set; } = string.Empty;
		public string? RepositoryURL { get; set; }
		public string? SiteURL { get; set; }
		public bool IsDeployed { get; set; } = false;
		public List<string>? Hosting { get; set; }
		public List<string> Tags { get; set; } = new List<string>();
		public bool Published { get; set; } = false;
		public bool Featured { get; set; } = false;
		public List<string>? RepositoryEvents { get; set; }
		public List<string>? DeploymentEvents { get; set; }
		public string? ETag { get; set; }

		public override string ToString() =>
			$"{Uid} | {ProjectName} - Last Updated {LastUpdated}";

	}
}
