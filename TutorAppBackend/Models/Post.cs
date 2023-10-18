using System.Xml.Linq;

namespace TutorAppBackend.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Content { get; set; } 
        public string? ImageUrl { get; set; } 
        public int LikesCount { get; set; }
        public DateTime PostedDate { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; } 
    }
}
