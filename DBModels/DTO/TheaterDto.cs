using DBModels.Models;
using System.ComponentModel.DataAnnotations;

namespace TheaterService.DTOs
{
    public class TheaterDto
    {
        public int TheaterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Capacity { get; set; }
    }
}
