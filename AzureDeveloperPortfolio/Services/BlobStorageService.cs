using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureDeveloperPortfolio.Data;

namespace AzureDeveloperPortfolio.Services
{
	public class BlobStorageService : IBlobStorageService
	{
		private static readonly string screenshots = nameof(screenshots);
		private readonly BlobContainerClient _blobContainerClient;
		public BlobStorageService(BlobServiceClient blobServiceClient)
		{
			_blobContainerClient = blobServiceClient.GetBlobContainerClient(screenshots);
		}


		public string? GetImage(string imageName)
		{
			try
			{
				Response<BlobDownloadResult>? result = _blobContainerClient.GetBlobClient(imageName).DownloadContent();

				byte[] imageBytes = result.Value.Content.ToArray();
				return Convert.ToBase64String(imageBytes);

			}
			catch (AuthenticationFailedException e)
			{
				Console.WriteLine($"Authentication Failed. {e.Message}");
				return null;
			}
		}



	}
}
