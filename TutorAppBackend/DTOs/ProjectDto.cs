using System.Collections.Generic;

namespace TutorAppBackend.DTOs
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOpenToRequests { get; set; }
        public UserDto Creator { get; set; }
        public List<UserDto> Members { get; set; }
    }
}
