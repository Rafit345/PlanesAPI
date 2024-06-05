using System;

namespace ZeroZen.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Brand { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
