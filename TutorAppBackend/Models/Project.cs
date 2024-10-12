using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorAppBackend.Models
{
    public class Project
    {
        public string Id { get; set; } 
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOpenToRequests { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public string CreatorId { get; set; }
        public User Creator { get; set; }
        public ICollection<ProjectMember> Members { get; set; }
    }
}
