using DogApp.Data;
using DogApp.Models.DTO;
using DogApp.Models;
using DogApp.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogApp.NUnitTests
{
    [TestFixture]
    public class DogManagerTests
    {
        private ApplicationDbContext _context;
        private IDog _dogManager;

        public enum SortOrder
        {
            Ascending,
            Descending
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "InMemoryDb")
                    .Options;

            _context = new ApplicationDbContext(options);
            _dogManager = new DogManager(_context);

            // Initialize the in-memory database with some data
            _context.Dogs.AddRange(new List<Dog>
            {
                new Dog { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                new Dog { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 },
                new Dog { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetDogsAsync_ReturnsAllDogs()
        {
            // Act
            var result = await _dogManager.GetDogsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task CreateDogAsync_CreatesDog()
        {
            // Arrange
            var newDog = new DogDTO
            {
                Name = "Max",
                Color = "Golden",
                TailLength = 12.0,
                Weight = 28.8
            };

            // Act
            _context.Dogs.RemoveRange(_context.Dogs);
            _context.SaveChanges();
            await _dogManager.CreateDogAsync(newDog);

            // Assert
            var result = await _dogManager.GetDogsAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(d => d.Name == newDog.Name));
        }

        [Test]
        public async Task GetPingAsync_ReturnsPing()
        {
            // Act
            var result = await _dogManager.GetPingAsync();

            // Assert
            Assert.AreEqual("Dogshouseservice.Version1.0.1", result);
        }

        [Test]
        public async Task SortByAttributeAsync_SortsDogs()
        {
            // Arrange
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 },
                new DogDTO { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
            };

            // Act
            var resultAsc = await _dogManager.SortByAttributeAsync(dogs, "Name", "asc");
            var resultDesc = await _dogManager.SortByAttributeAsync(dogs, "Name", "desc");

            // Assert
            Assert.IsTrue(IsSorted(resultAsc.Select(d => d.Name), SortOrder.Ascending));
            Assert.IsTrue(IsSorted(resultDesc.Select(d => d.Name), SortOrder.Descending));
        }

        private bool IsSorted(IEnumerable<string> sequence, SortOrder sortOrder)
        {
            var sorted = sortOrder == SortOrder.Ascending
                ? sequence.OrderBy(s => s)
                : sequence.OrderByDescending(s => s);

            return sequence.SequenceEqual(sorted);
        }
    }
}

