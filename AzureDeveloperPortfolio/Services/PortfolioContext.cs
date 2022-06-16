using AzureDeveloperPortfolio.Data;
using Microsoft.EntityFrameworkCore;

namespace AzureDeveloperPortfolio.Services
{
	public sealed class PortfolioContext : DbContext
	{
		public PortfolioContext(DbContextOptions<PortfolioContext> options)
			: base(options)
		{ }

		public DbSet<Project> Projects => Set<Project>();
		public DbSet<Tag> Tags => Set<Tag>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasManualThroughput(400);

			modelBuilder.Entity<Project>()
				.HasNoDiscriminator()
				.ToContainer(nameof(Projects))
				.HasKey(p => p.Uid);

			modelBuilder.Entity<Project>()
				.HasPartitionKey(p => p.Uid)
				.Property(p => p.ETag)
				.IsETagConcurrency();

			modelBuilder.Entity<Tag>()
				.HasNoDiscriminator()
				.ToContainer(nameof(Tags))
				.HasKey(t => t.TagName);

			modelBuilder.Entity<Tag>()
				.HasPartitionKey(t => t.TagName)
				.Property(t => t.ETag)
				.IsETagConcurrency();
		}
	}
}