using System.Collections.Generic;

namespace TutorAppBackend.DTOs
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ServiceTierDTO> Tiers { get; set; }
    }
}