using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace InvoiceBillingSystem.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService()
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/credentials/google-drive-service-account.json");

            GoogleCredential credential;
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.DriveFile);
            }

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "InvoiceBillingSystem"
            });
        }

        public async Task<string> UploadFileAsync(byte[] fileData, string fileName, string folderId)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Parents = new List<string> { folderId }  // Folder ID
            };

            using (var stream = new MemoryStream(fileData))
            {
                var request = _driveService.Files.Create(fileMetadata, stream, "application/pdf");
                request.Fields = "id";
                var response = await request.UploadAsync();

                if (response.Status == UploadStatus.Completed)
                {
                    string fileId = request.ResponseBody.Id;
                    return $"https://drive.google.com/uc?id={fileId}&export=download";  // Public link format
                }
                else
                {
                    throw new Exception("File upload failed.");
                }
            }
        }


        public async Task MakeFilePublicAsync(string fileId)
        {
            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Type = "anyone",
                Role = "reader"
            };

            await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();
        }
    }
}
