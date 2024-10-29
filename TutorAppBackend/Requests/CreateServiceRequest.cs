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
        public List<CreateServiceTierRequest> Tiers { get; set; }
    }
}