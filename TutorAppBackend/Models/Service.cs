﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace TutorAppBackend.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<ServiceTier> Tiers { get; set; }
    }
}
