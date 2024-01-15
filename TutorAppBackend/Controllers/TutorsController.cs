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


[ApiController]
[Route("api/[controller]")]
public class TutorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public TutorsController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _configuration = configuration;
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
        if (identityResult.Succeeded)
        {
            return CreatedAtAction(nameof(GetTutors), new
            {
                userId = user.Id, // Include the user's ID in the response
                message = "User registered successfully"
            });
        }
        return BadRequest(identityResult.Errors);
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
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured correctly in appsettings.");
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

    [HttpGet("search")]
    public ActionResult<IEnumerable<User>> SearchUsers(string name)
    {
        var users = _context.User.Where(u => u.FullName != null && u.FullName.Contains(name)).ToList();
        return users;
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
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Update the user properties
        user.Name = request.Name;
        user.Bio = request.Bio;
        user.Website = request.Website;
        user.School = request.School;
        user.Grade = request.Grade;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Profile updated successfully" });
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


}