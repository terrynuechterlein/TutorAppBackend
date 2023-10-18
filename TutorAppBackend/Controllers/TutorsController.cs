using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using TutorAppBackend.Data;
using TutorAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorAppBackend.Requests;

[ApiController]
[Route("api/[controller]")]
public class TutorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public TutorsController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;

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
            return CreatedAtAction(nameof(GetTutors), user);
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

        if (!signInResult.Succeeded)
        {
            return BadRequest(new { message = "Invalid email or password" });
        }

        return Ok(new { message = "Logged in successfully" });
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



}