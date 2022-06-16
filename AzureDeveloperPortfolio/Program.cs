using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContextFactory<PortfolioContext>(
	(IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
	{
		options.UseCosmos(
			builder.Configuration["Cosmos:EndPoint"],
			builder.Configuration["Cosmos:AccessKey"],
			builder.Configuration["Cosmos:DatabaseName"]
			?? throw new InvalidOperationException("Connection string 'PortfolioContext' not found."));
	});
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
}

using IServiceScope? scope = app.Services.CreateScope();
PortfolioContext? context = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
IConfiguration config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
if (config is not null)
{
	context.Tags.Add(new Tag(config["Database:DefaultTag"]));
	context.SaveChangesAsync();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
