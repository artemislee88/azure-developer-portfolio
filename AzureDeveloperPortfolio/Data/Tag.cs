namespace AzureDeveloperPortfolio.Data
{
	public class Tag
	{
		public Tag(string tagName)
		{
			TagName = tagName;
			Projects = new List<ProjectSummary>();
		}

		public string TagName { get; set; }
		public string? ETag { get; set; }
		public List<ProjectSummary> Projects { get; set; } = new();

		public override string ToString() =>
			$"{TagName} tagged by {Projects.Count} projects.";
	}
}
