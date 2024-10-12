using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TutorAppBackend.Data;
using TutorAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorAppBackend.DTOs;
using TutorAppBackend.Requests;
using Microsoft.Extensions.Logging;
using TutorAppBackend.Services;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ProjectsController> _logger;
    private readonly BlobService _blobService;

    public ProjectsController(AppDbContext context, UserManager<User> userManager, ILogger<ProjectsController> logger, BlobService blobService)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
        _blobService = blobService;
    }

    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var creator = await _userManager.FindByIdAsync(request.CreatorId);
        if (creator == null)
        {
            return NotFound("Creator not found.");
        }

        var project = new Project
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            IsOpenToRequests = request.IsOpenToRequests,
            CreatorId = creator.Id,
            Members = new List<ProjectMember>()
        };

        // Add the creator as a member
        project.Members.Add(new ProjectMember { ProjectId = project.Id, UserId = creator.Id });

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Project created successfully.", projectId = project.Id });
    }

    // GET: api/projects
    [HttpGet]
    public IActionResult GetProjects()
    {
        var projects = _context.Projects
            .Include(p => p.Creator)
            .Include(p => p.Members).ThenInclude(pm => pm.User)
            .ToList();

        var projectDtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsOpenToRequests = p.IsOpenToRequests,
            Creator = new UserDto
            {
                Id = p.Creator.Id,
                UserName = p.Creator.UserName,
                ProfilePictureUrl = !string.IsNullOrEmpty(p.Creator.ProfilePictureUrl)
                    ? _blobService.GenerateBlobSasToken("profileimages", Path.GetFileName(new Uri(p.Creator.ProfilePictureUrl).LocalPath))
                    : null
            },
            Members = p.Members.Select(pm => new UserDto
            {
                Id = pm.User.Id,
                UserName = pm.User.UserName,
                ProfilePictureUrl = !string.IsNullOrEmpty(pm.User.ProfilePictureUrl)
                    ? _blobService.GenerateBlobSasToken("profileimages", Path.GetFileName(new Uri(pm.User.ProfilePictureUrl).LocalPath))
                    : null
            }).ToList()
        }).ToList();

        return Ok(projectDtos);
    }

    // POST: api/projects/{projectId}/requestToJoin
    [HttpPost("{projectId}/requestToJoin")]
    public async Task<IActionResult> RequestToJoin(string projectId, [FromBody] RequestToJoinRequest request)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return NotFound("Project not found.");
        }

        if (!project.IsOpenToRequests)
        {
            return BadRequest("This project is not open to requests.");
        }

        // Check if user is already a member
        var isAlreadyMember = await _context.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == request.UserId);
        if (isAlreadyMember)
        {
            return BadRequest("User is already a member of this project.");
        }

        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = request.UserId
        };

        _context.ProjectMembers.Add(projectMember);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Request to join project successful." });
    }
}
