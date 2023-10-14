using System.ComponentModel.DataAnnotations;

namespace DogApp.Models.DTO
{
    public class DogDTO
    {
        public string? Name { get; set; }

        public string? Color { get; set; }

        [Required(ErrorMessage = "The TailLength field is required.")]
        public double TailLength { get; set; }

        [Required(ErrorMessage = "The Weight field is required.")]
        public double Weight { get; set; }
    }
}

