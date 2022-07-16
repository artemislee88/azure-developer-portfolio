using Azure.Identity;
using AzureDeveloperPortfolio.Data;
using AzureDeveloperPortfolio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

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
IConfiguration storage = builder.Configuration.GetSection("Storage:ServiceUrl");
builder.Services.AddAzureClients(builder =>
{

	//builder.UseCredential(new DefaultAzureCredential());

	builder.AddBlobServiceClient(new Uri("https://127.0.0.1:10000/")).WithCredential(new DefaultAzureCredential());

});
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();


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
IDbContextFactory<PortfolioContext> contextfactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<PortfolioContext>>();
PortfolioContext context = contextfactory.CreateDbContext();

//await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();
if (context.Projects.Count() == 0)
{
	DBInitializer.Initialize(contextfactory);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
