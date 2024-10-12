namespace TutorAppBackend.Requests
{
    public class CreateProjectRequest
    {
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOpenToRequests { get; set; }
    }
}
