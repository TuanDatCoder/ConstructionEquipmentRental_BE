using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Services.FirebaseStorageServices
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucketName;
        private readonly string _credentialsFilePath;
        private StorageClient _storageClient;

        public FirebaseStorageService(IConfiguration configuration)
        {
            // Lấy thông tin cấu hình từ appsettings.json
            _bucketName = configuration.GetValue<string>("Firebase:BucketName");
            var serviceKeyPath = configuration.GetValue<string>("Firebase:ServiceKeyPath");

            if (string.IsNullOrEmpty(_bucketName) || string.IsNullOrEmpty(serviceKeyPath))
            {
                throw new InvalidOperationException("Missing Firebase configuration (BucketName or ServiceKeyPath).");
            }

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            _credentialsFilePath = Path.Combine(basePath, serviceKeyPath);

            InitializeStorage();
        }

        /// <summary>
        /// Tải tệp lên Firebase Storage.
        /// </summary>
        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("File stream cannot be null or empty.");
            }

            string uniqueFileName = GenerateUniqueFileName(originalFileName);

            await _storageClient.UploadObjectAsync(
                _bucketName,
                uniqueFileName,
                null,
                fileStream);

            return uniqueFileName;
        }

        /// <summary>
        /// Tạo URL có chữ ký để truy cập tệp.
        /// </summary>
        public string GetSignedUrl(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.");
            }

            var urlSigner = UrlSigner.FromServiceAccountPath(_credentialsFilePath);

            return urlSigner.Sign(
                _bucketName,
                fileName,
                TimeSpan.FromDays(7),
                HttpMethod.Get);
        }

        /// <summary>
        /// Khởi tạo StorageClient.
        /// </summary>
        private void InitializeStorage()
        {
            GoogleCredential googleCredential;
            using (var jsonStream = new FileStream(_credentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(jsonStream);
            }

            _storageClient = StorageClient.Create(googleCredential);

            // Kiểm tra bucket tồn tại
            var bucket = _storageClient.GetBucket(_bucketName);
            if (bucket == null)
            {
                throw new InvalidOperationException($"Bucket '{_bucketName}' not found.");
            }
        }

        /// <summary>
        /// Tạo tên tệp duy nhất dựa trên GUID.
        /// </summary>
        private string GenerateUniqueFileName(string originalFileName)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(originalFileName);
            return $"{uniqueId}{extension}";
        }

        /// <summary>
        /// Resize hình ảnh trước khi upload.
        /// </summary>
        private Stream ResizeImage(Stream inputStream, int maxWidth = 800, int maxHeight = 800)
        {
            // Đảm bảo Stream bắt đầu từ vị trí đầu
            if (inputStream.CanSeek)
            {
                inputStream.Position = 0;
            }

            try
            {
                // Tải ảnh từ stream
                using var image = Image.Load(inputStream);
                int newWidth = image.Width;
                int newHeight = image.Height;

                // Kiểm tra và tính toán kích thước mới
                if (image.Width > maxWidth || image.Height > maxHeight)
                {
                    if (image.Width > image.Height)
                    {
                        // Nếu chiều rộng lớn hơn chiều cao, điều chỉnh chiều rộng
                        newWidth = maxWidth;
                        newHeight = image.Height * maxWidth / image.Width;
                    }
                    else
                    {
                        // Nếu chiều cao lớn hơn chiều rộng, điều chỉnh chiều cao
                        newHeight = maxHeight;
                        newWidth = image.Width * maxHeight / image.Height;
                    }
                }

                // Resize ảnh
                image.Mutate(x => x.Resize(newWidth, newHeight));

                // Lưu ảnh vào MemoryStream
                var outputStream = new MemoryStream();
                image.Save(outputStream, new JpegEncoder()); // Lưu ảnh vào stream với định dạng JPEG
                outputStream.Position = 0; // Reset vị trí của stream trước khi trả về

                return outputStream;
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, ném ra exception mới với thông báo lỗi
                throw new InvalidOperationException($"Error resizing image: {ex.Message}", ex);
            }
        }

    }
}
