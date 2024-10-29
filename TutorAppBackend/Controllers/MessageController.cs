using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TutorAppBackend.Data;
using TutorAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutorAppBackend.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly BlobService _blobService;
    public MessagesController(AppDbContext context, UserManager<User> userManager, BlobService blobService)
    {
        _context = context;
        _userManager = userManager;
        _blobService = blobService;
    }

    // POST: api/messages
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var senderId = User.FindFirstValue("uid");

        if (senderId == null)
        {
            return Unauthorized();
        }

        var receiver = await _userManager.FindByIdAsync(request.ReceiverId);
        if (receiver == null)
        {
            return NotFound("Receiver not found");
        }

        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            Timestamp = DateTime.UtcNow
        };

        _context.Message.Add(message);
        await _context.SaveChangesAsync();

        var messageDto = new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            Timestamp = message.Timestamp
        };

        return Ok(messageDto);
    }

    // GET: api/messages/conversation/{otherUserId}
    [HttpGet("conversation/{otherUserId}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversation(string otherUserId)
    {
        var currentUserId = User.FindFirstValue("uid");

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var messages = await _context.Message
            .Where(m =>
                (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                Timestamp = m.Timestamp
            })
            .ToListAsync();

        return Ok(messages);
    }

    private string GenerateSasUrl(string blobUrl)
    {
        if (string.IsNullOrEmpty(blobUrl))
            return null;

        var uri = new Uri(blobUrl);
        var blobName = Path.GetFileName(uri.LocalPath);
        var sasUrl = _blobService.GenerateBlobSasToken("ContainerName", blobName);
        return sasUrl;
    }


    // GET: api/messages/conversations
    [HttpGet("conversations")]
    public async Task<ActionResult<IEnumerable<ConversationDto>>> GetConversations()
    {
        var currentUserId = User.FindFirstValue("uid");

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var messages = await _context.Message
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToListAsync();

        var conversations = messages
            .Select(m => new
            {
                OtherUser = m.SenderId == currentUserId ? m.Receiver : m.Sender,
                Message = m
            })
            .GroupBy(m => m.OtherUser.Id)
            .Select(g => new ConversationDto
            {
                UserId = g.Key,
                UserName = g.First().OtherUser.UserName,
                FirstName = g.First().OtherUser.FirstName,
                LastName = g.First().OtherUser.LastName,
                ProfilePictureUrl = GenerateSasUrl(g.First().OtherUser.ProfilePictureUrl),
                LastMessage = g.OrderByDescending(m => m.Message.Timestamp).First().Message.Content,
                Timestamp = g.OrderByDescending(m => m.Message.Timestamp).First().Message.Timestamp
            })
            .OrderByDescending(c => c.Timestamp)
            .ToList();

        return Ok(conversations);
    }
}


public class SendMessageRequest
{
    public string ReceiverId { get; set; }
    public string Content { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ConversationDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string LastMessage { get; set; }
    public DateTime Timestamp { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

