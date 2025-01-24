using Microsoft.AspNetCore.Mvc;
using Services.FirebaseStorageServices;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFirebaseStorageService _firebaseStorageService;

        public FileController(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected.");
            }

            using var stream = file.OpenReadStream();
            string fileName = await _firebaseStorageService.UploadFileAsync(stream, file.FileName);
            string signedUrl = _firebaseStorageService.GetSignedUrl(fileName);

            return Ok(new { FileName = fileName, Url = signedUrl });
        }
    }
}
