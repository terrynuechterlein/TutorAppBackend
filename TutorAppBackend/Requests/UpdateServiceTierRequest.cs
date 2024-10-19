using System.ComponentModel.DataAnnotations;

namespace TutorAppBackend.Requests
{
    public class UpdateServiceTierRequest
    {
        public int? Id { get; set; } // To identify existing tiers

        public string Title { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }
    }
}
