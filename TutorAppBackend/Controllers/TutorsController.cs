using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using TutorAppBackend.Data;
using TutorAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorAppBackend.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using TutorAppBackend.DTOs;
using Microsoft.Extensions.Logging; 
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;

[ApiController]
[Route("api/[controller]")]
public class TutorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TutorsController> _logger; 


    public TutorsController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ILogger<TutorsController> logger)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _configuration = configuration;
        _logger = logger;

    }

    // GET: api/users/tutors
    [HttpGet("tutors")]
    public ActionResult<IEnumerable<User>> GetTutors()
    {
        var tutors = _context.User.Where(u => u.IsTutor == true).ToList();
        return tutors;
    }

    // POST: api/tutors/register
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterRequest request)
    {
        // Check if the model is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Password is required" });
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            IsTutor = request.IsTutor
        };

        var identityResult = await _userManager.CreateAsync(user, request.Password);

        _logger.LogInformation($"Registration attempt with email: {request.Email}");

        if (identityResult.Succeeded)
        {
            // Respond with JSON including the user ID
            return CreatedAtAction(nameof(GetTutors), new
            {
                userId = user.Id,
                message = "User registered successfully"
            });
        }

        // Respond with errors in JSON format
        _logger.LogWarning($"Registration failed for user: {user.UserName}");

        return BadRequest(new { message = "Registration failed", errors = identityResult.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { message = "Email is required" });
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest(new { message = "Invalid email or password" });
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Password is required" });
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (signInResult.Succeeded)
        {
            //create the token
            var token = GenerateJwtToken(user); 

            return Ok(new
            {
                token = token,
                userId = user.Id, // Include the user's ID in the response
                message = "Logged in successfully"
            });
        }

        return BadRequest(new { message = "Invalid email or password" });
    }

    private string GenerateJwtToken(User user)
    {
        // Fetch the JWT key from appsettings.json
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key is not configured correctly in appsettings.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("uid", user.Id) // Custom claim to store the user's ID
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // PUT: api/tutors/{id}/becomeTutor
    [HttpPut("{id}/becomeTutor")]
    public async Task<IActionResult> BecomeTutor(string id)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.IsTutor = true;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
  
    [HttpGet("feed")]
    public ActionResult<IEnumerable<Post>> GetUserFeed(string userId)
    {
        var followedUsers = _context.User.Include(u => u.Following).FirstOrDefault(u => u.Id == userId)?.Following;

        if (followedUsers == null || !followedUsers.Any())
        {
            return NotFound("The user does not exist or is not following anyone.");
        }

        var posts = _context.Posts
            .Where(p => followedUsers.Any(f => f.Id == p.UserId))
            .ToList();

        return posts;
    }

    [HttpPost("post")]
    public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPost", new { id = post.Id }, post);
    }

    // POST: api/tutors/uploadProfilePicture
    [HttpPost("{id}/uploadProfilePicture")]
    public async Task<IActionResult> UploadProfilePicture(string id, [FromForm] IFormFile profileImage)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (profileImage != null && profileImage.Length > 0)
        {
            var imageUrl = await SaveImageToAzureBlob(profileImage, "profileimages");
            user.ProfilePictureUrl = imageUrl;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile picture updated successfully" });
        }

        return BadRequest("Invalid image file");
    }

    // POST: api/tutors/{id}/uploadBannerImage
    [HttpPost("{id}/uploadBannerImage")]
    public async Task<IActionResult> UploadBannerImage(string id, [FromForm] IFormFile bannerImage)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (bannerImage != null && bannerImage.Length > 0)
        {
            var imageUrl = await SaveImageToAzureBlob(bannerImage, "bannerimages");
            user.BannerImageUrl = imageUrl; 
            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Banner image updated successfully" });
        }

        return BadRequest("Invalid image file");
    }

    private async Task<string> SaveImageToAzureBlob(IFormFile imageFile, string containerName)
    {
        var connectionString = _configuration["AzureStorage:ConnectionString"];
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await using (var stream = imageFile.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.ToString();
    }


    [HttpGet("validateToken")]
    public IActionResult ValidateToken()
    {
        // If the user reaches this point, the token is valid 
        // because the JWT middleware has already validated it.
        return Ok(new { message = "Token is valid" });
    }

    // PUT: api/tutors/{id}/updateProfile
    [HttpPut("{id}/updateProfile")]
    public async Task<IActionResult> UpdateProfile(string id, [FromBody] UpdateUserRequest request)
    {
        _logger.LogInformation($"UpdateProfile called for user ID {id} with request: {JsonConvert.SerializeObject(request)}");

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Update the user properties
        if (request.UserName != null) user.UserName = request.UserName;
        if (request.Major != null) user.Major = request.Major;
        if (request.Website != null) user.Website = request.Website;
        if (request.School != null) user.School = request.School;
        if (request.Grade != null) user.Grade = request.Grade;
        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.YoutubeUrl != null) user.YoutubeUrl = request.YoutubeUrl;
        if (request.TwitchUrl != null) user.TwitchUrl = request.TwitchUrl;
        if (request.DiscordUrl != null) user.DiscordUrl = request.DiscordUrl;
        if (request.LinkedInUrl != null) user.LinkedInUrl = request.LinkedInUrl;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Profile updated successfully" });
    }

    // PUT: api/tutors/{id}/initialSetup
    [HttpPut("{id}/initialSetup")]
    public async Task<IActionResult> InitialSetup(string id, [FromBody] InitialSetupRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.School = request.School;
        user.Major = request.Major;

        user.IsSetupComplete = true; // Mark the initial setup as complete

        await _userManager.UpdateAsync(user);

        return Ok(new { message = "Initial setup completed successfully." });
    }

    private string GenerateBlobSasToken(string containerName, string blobName)
    {
        var blobServiceClient = new BlobServiceClient(_configuration["AzureStorage:ConnectionString"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b", // b for blob
            StartsOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddHours(24) // Token valid for 24 hours
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

        return $"{blobClient.Uri}{sasToken}";
    }


    [HttpGet("{id}/profileImage")]
    public async Task<IActionResult> GetProfileImageUrl(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(user.ProfilePictureUrl))
        {
            return Ok(new { imageUrl = "" });
        }

        var uri = new Uri(user.ProfilePictureUrl);
        var blobName = Path.GetFileName(uri.LocalPath);
        var sasUrl = GenerateBlobSasToken("profileimages", blobName);

        return Ok(new { imageUrl = sasUrl });
    }

    [HttpGet("{id}/bannerImage")]
    public async Task<IActionResult> GetBannerImageUrl(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(user.BannerImageUrl))
        {
            return Ok(new { imageUrl = "" });
        }

        var uri = new Uri(user.BannerImageUrl);
        var blobName = Path.GetFileName(uri.LocalPath);
        var sasUrl = GenerateBlobSasToken("bannerimages", blobName);

        return Ok(new { imageUrl = sasUrl });
    }

    [HttpGet("{id}/profile")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile(string id)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var userProfileDto = new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Website = user.Website,
            School = user.School,
            Grade = user.Grade,
            Major = user.Major,
            Bio = user.Bio,
            YoutubeUrl = user.YoutubeUrl,
            TwitchUrl = user.TwitchUrl,
            DiscordUrl = user.DiscordUrl,
            LinkedInUrl = user.LinkedInUrl,
            IsSetupComplete = user.IsSetupComplete,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        return Ok(userProfileDto);
    }

    // PUT: api/tutors/{id}/updateBio
    [HttpPut("{id}/updateBio")]
    public async Task<IActionResult> UpdateBio(string id, [FromBody] UpdateBioRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Bio = request.Bio;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(new { message = "Bio updated successfully." });
        }
        return BadRequest("Failed to update bio.");
    }

    // GET: api/tutors/allUsers
    [HttpGet("allusers")]
    public ActionResult<IEnumerable<GetAllUsersDTO>> GetAllUsers(
            [FromQuery] string[] college,
            [FromQuery] string[] grade,
            [FromQuery] string[] major,
            [FromQuery] string searchQuery = null) 

    {
        // Log the received filter parameters for debugging
        _logger.LogInformation($"Received filter parameters. College: {string.Join(", ", college)}, Grade: {string.Join(", ", grade)}, Major: {string.Join(", ", major)}");

        var query = _context.Users.AsQueryable();

        // Filter by college if provided
        if (college != null && college.Any())
        {
            query = query.Where(user => college.Contains(user.School));
        }

        // Filter by grade if provided
        if (grade != null && grade.Any())
        {
            query = query.Where(user => grade.Contains(user.Grade));
        }

        // Filter by major if provided
        if (major != null && major.Any())
        {
            query = query.Where(user => major.Contains(user.Major));
        }

        // New filtering logic for searchQuery
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(user =>
                user.FirstName.Contains(searchQuery) ||
                user.LastName.Contains(searchQuery) ||
                user.UserName.Contains(searchQuery));

        }


        // Fetch the users without generating SAS tokens
        var users = query.Select(user => new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.ProfilePictureUrl,
            user.BannerImageUrl,
            user.School,
            user.Grade,
            user.Major,
            user.Bio
        }).ToList(); // Execute the query and materialize results

        // Generate SAS tokens for each user in memory
        var usersWithSasTokens = users.Select(user => new GetAllUsersDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            ProfilePictureUrl = user.ProfilePictureUrl != null ? GenerateBlobSasToken("profileimages", Path.GetFileName(new Uri(user.ProfilePictureUrl).LocalPath)) : null,
            BannerImageUrl = user.BannerImageUrl != null ? GenerateBlobSasToken("bannerimages", Path.GetFileName(new Uri(user.BannerImageUrl).LocalPath)) : null,
            School = user.School,
            Grade = user.Grade,
            Major = user.Major,
            Bio = user.Bio,
            // FollowersCount and FollowingCount can be added here if needed
        }).ToList();

        return Ok(usersWithSasTokens);
    }




}