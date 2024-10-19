using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TutorAppBackend.Requests;

namespace TutorAppBackend.Requests
{
    public class CreateServiceRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public List<CreateServiceTierRequest> Tiers { get; set; }
    }
}