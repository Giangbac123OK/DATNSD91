using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImagesController : Controller
	{
		private readonly IWebHostEnvironment _environment;

		public ImagesController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("Không có file nào được tải lên.");

			if (string.IsNullOrEmpty(_environment.WebRootPath))
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Web root path không được cấu hình.");
			}

			var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
			return Ok(new { Url = fileUrl });
		}

	}
}
