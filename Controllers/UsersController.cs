using LearnSmartCoding.CosmosDb.Linq.API.Core;
using LearnSmartCoding.CosmosDb.Linq.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSmartCoding.CosmosDb.Linq.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDocument>> GetUser(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<UserDocument>> CreateUser(UserDocument user)
        {
            // Set any additional properties if required

            var createdUser = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { userId = createdUser.Id }, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDocument>> UpdateUser(string userId, UserDocument user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }

            user.Id = existingUser.Id; // Preserve the original ID

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(userId);
            return NoContent();
        }

        [HttpPost("upload/{username}")]
        public IActionResult UploadProfileImage(string username, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            //"/app/uploads" this location is virtual, we will map this to azure file share in this video
            // Generate a unique filename with incremental number
            var uniqueFileName = $"{username}_{DateTime.Now.Ticks}_{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine("/app/uploads", uniqueFileName);


            try
            {
                // Save the uploaded file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Ok("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Handle any error that occurred during file upload
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload image." + ex.ToString());
            }
        }

        [HttpGet("users/{username}/images")]
        public IActionResult GetUserImages(string username)
        {
            try
            {
                var uploadDirectory = "/app/uploads";
                var userImages = Directory.GetFiles(uploadDirectory, $"{username}_*.jpg")
                                          .Select(Path.GetFileName);
                var imageLinks = userImages.Select(fileName => $"/uploads/{fileName}");
                // Assuming "/uploads" is the endpoint serving the images

                return Ok(imageLinks);
            }
            catch (Exception ex)
            {
                // Handle any error that occurred during file upload
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve uploaded images." + ex.ToString());
            }
        }

    }
}
