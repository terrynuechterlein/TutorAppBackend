﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TutorAppBackend.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public string? School { get; set; }
        public string? Grade { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsTutor { get; set; }
        public int ClassYear { get; set; }
        public int Popularity { get; set; }
        public ICollection<Subject>? Subjects { get; set; } 
        public virtual ICollection<User>? Followers { get; set; } 
        public virtual ICollection<User>? Following { get; set; }
                                                                 
    }
}
