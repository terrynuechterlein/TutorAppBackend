using System.ComponentModel.DataAnnotations;

namespace TutorAppBackend.Requests
{
    public class CreateServiceTierRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}