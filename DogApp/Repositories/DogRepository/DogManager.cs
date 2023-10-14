using DogApp.Data;
using DogApp.Models;
using DogApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

            order = order.ToLower();
            
            // Проверьте, что атрибут сортировки существует в модели Dog
            if (typeof(DogDTO).GetProperty(attribute) == null)
            {
                throw new ArgumentException("Invalid sorting attribute.");
            }

            // Преобразуйте направление сортировки из строки в перечисление SortOrder
            if (order != "asc" && order != "desc")
            {
                throw new ArgumentException("Invalid sorting order. Use 'asc' or 'desc'.");
            }

            // Выполните сортировку в зависимости от параметров запроса
            if (order == "asc")
            {
                dogs = dogs.OrderBy(d => typeof(DogDTO).GetProperty(attribute).GetValue(d, null)).ToList();
            }
            else
            {
                dogs = dogs.OrderByDescending(d => typeof(DogDTO).GetProperty(attribute).GetValue(d, null)).ToList();
            }

            return dogs;
        }

        public async Task<bool> IsDogNameUniqueAsync(string name)
        {
            // Check if there is a dog with the same name in the database
            return await _db.Dogs.AnyAsync(d => d.Name == name);
        }
    }
}
