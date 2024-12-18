﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace TutorAppBackend.Models
{
    public class ServiceTier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Price { get; set; }
        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
