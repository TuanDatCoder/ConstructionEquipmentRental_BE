using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.FirebaseStorageServices
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucketName = "marinepath-56521.appspot.com";
        private readonly string _credentialsFilePath;
        private StorageClient _storageClient;

        public FirebaseStorageService(IConfiguration configuration)
        {
            var serviceKeyPath = configuration.GetValue<string>("Firebase:ServiceKeyPath");
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            _credentialsFilePath = Path.Combine(basePath, serviceKeyPath);
            InitializeStorage();
        }

        //private void InitializeStorage()
        //{
        //    GoogleCredential googleCredential;
        //    using (var jsonStream = new FileStream(_credentialsFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        googleCredential = GoogleCredential.FromStream(jsonStream);
        //    }
        //    _storageClient = StorageClient.Create(googleCredential);
        //}

        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName)
        {
            string uniqueFileName = GenerateUniqueFileName(originalFileName);
            var storageObject = await _storageClient.UploadObjectAsync(
                _bucketName,
                uniqueFileName,
                null,
                fileStream);
            return uniqueFileName;
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string uniqueId = Guid.NewGuid().ToString();
            return $"{uniqueId}_{originalFileName}";
        }

        public string GetSignedUrl(string fileName)
        {
            var urlSigner = UrlSigner.FromServiceAccountPath(_credentialsFilePath);
            string signedUrl = urlSigner.Sign(
                _bucketName,
                fileName,
                TimeSpan.FromDays(7),
                HttpMethod.Get);
            return signedUrl;
        }


        private void InitializeStorage()
        {
            GoogleCredential googleCredential;
            using (var jsonStream = new FileStream(_credentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(jsonStream);
            }

            _storageClient = StorageClient.Create(googleCredential);

            // Verify if the bucket exists
            try
            {
                var bucket = _storageClient.GetBucket(_bucketName);
            }
            catch (Google.GoogleApiException e) when (e.Error.Code == 404)
            {
            }
        }

    }
}
