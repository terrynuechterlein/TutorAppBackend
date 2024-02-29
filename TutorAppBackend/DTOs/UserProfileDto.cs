namespace TutorAppBackend.DTOs
{
    public class UserProfileDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Website { get; set; }
        public string? School { get; set; }
        public string? Grade { get; set; }
        public string? Major { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? TwitchUrl { get; set; }
        public string? DiscordUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsSetupComplete { get; set; }

    }
}
