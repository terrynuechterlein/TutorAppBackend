using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorAppBackend.Requests
{
    public class UpdateServiceRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        public List<UpdateServiceTierRequest> Tiers { get; set; }
    }
}
