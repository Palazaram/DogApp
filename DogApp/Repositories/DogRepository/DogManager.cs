using DogApp.Data;
using DogApp.Models;
using DogApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DogApp.Repositories.DogRepository
{
    public class DogManager : IDog
    {
        private readonly ApplicationDbContext _db;

        public DogManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DogDTO>> GetDogsAsync()
        {
            IEnumerable<DogDTO> dogs = await (from dog in _db.Dogs
                                       select new DogDTO
                                       {
                                           Name = dog.Name,
                                           Color = dog.Color,
                                           TailLength = dog.TailLength,
                                           Weight = dog.Weight
                                       }).ToListAsync();
            return dogs;
        }

        public async Task CreateDogAsync(DogDTO dog)
        {
            var newDog = new Dog
            {
                Name = dog.Name,
                Color = dog.Color,
                TailLength = dog.TailLength,
                Weight = dog.Weight
            };

            _db.Dogs.Add(newDog);
            await _db.SaveChangesAsync();
        }

        public async Task<string> GetPingAsync()
        {
            return await Task.FromResult("Dogshouseservice.Version1.0.1");
        }

        public async Task<IEnumerable<DogDTO>> SortByAttributeAsync(IEnumerable<DogDTO> dogs, string attribute, string order)
        {
            if (string.IsNullOrWhiteSpace(attribute) || string.IsNullOrWhiteSpace(order))
            {
                throw new ArgumentException("Both 'attribute' and 'order' parameters are required.");
            }

            attribute = attribute.ToLower();
            order = order.ToLower();

            // Checking that the sort attribute exists in the DogDTO model (case insensitive)
            PropertyInfo property = typeof(DogDTO).GetProperties()
                .FirstOrDefault(p => p.Name.ToLower() == attribute);

            // Checking that the sort attribute exists in the Dog model
            if (property == null)
            {
                throw new ArgumentException("Invalid sorting attribute. All attributes: Name, Color, TailLength, Weight.");
            }

            if (order != "asc" && order != "desc")
            {
                throw new ArgumentException("Invalid sorting order. Use 'asc' or 'desc'.");
            }

            if (order == "asc")
            {
                dogs = dogs.OrderBy(d => property.GetValue(d, null)).ToList();
            }
            else
            {
                dogs = dogs.OrderByDescending(d => property.GetValue(d, null)).ToList();
            }

            return dogs;
        }

        public async Task<bool> IsDogNameUniqueAsync(string name)
        {
            // Checking if there is a dog with the same name in the database
            return await _db.Dogs.AnyAsync(d => d.Name == name);
        }
    }
}
