namespace AzureDeveloperPortfolio.Data
{
	public class ProjectSummary : IEquatable<ProjectSummary>
	{
		public ProjectSummary()
		{
		}

		public ProjectSummary(Project project)
		{
			ProjectUid = project.Uid;
			ProjectName = project.ProjectName;
			ShortDescription = project.ShortDescription;
			ProfileScreenshot = project.ProfileScreenshot;
			LastUpdated = project.LastUpdated;
		}

		public string ProjectUid { get; set; } = string.Empty;
		public string ProjectName { get; set; } = string.Empty;
		public string ShortDescription { get; set; } = string.Empty;
		public string ProfileScreenshot { get; set; } = string.Empty;
		public DateTime LastUpdated { get; set; }

		public override bool Equals(object? obj)
		{
			return Equals(obj as ProjectSummary);
		}

		public bool Equals(ProjectSummary? other)
		{
			return other is not null &&
				   ProjectUid == other.ProjectUid &&
				   ProjectName == other.ProjectName &&
				   ShortDescription == other.ShortDescription &&
				   ProfileScreenshot == other.ProfileScreenshot &&
				   LastUpdated == other.LastUpdated;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ProjectUid, ProjectName, ShortDescription, ProfileScreenshot, LastUpdated);
		}

		public override string ToString() => $"{ProjectName} - {LastUpdated}";

	}
}
