namespace TutorAppBackend.Models
{
    public class ProjectMember
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
