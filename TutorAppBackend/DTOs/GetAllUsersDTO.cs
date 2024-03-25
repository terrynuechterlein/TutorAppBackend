namespace TutorAppBackend.DTOs
{
    public class GetAllUsersDTO
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? School { get; set; }
        public string? Grade { get; set; }
        public string? Major { get; set; }
        public string? Bio {  get; set; }
        // public int FollowersCount { get; set; } 
        // public int FollowingCount { get; set; } 
    }
}
