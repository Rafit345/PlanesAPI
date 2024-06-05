using System.ComponentModel.DataAnnotations;

namespace ZeroZen.Models
{
    public class PlaneDto
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Brand { get; set; } 
    }
}
