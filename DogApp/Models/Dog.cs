//using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace DogApp.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public double TailLength { get; set; }
        public double Weight { get; set; }
    }
}
