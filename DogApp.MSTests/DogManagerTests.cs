using DogApp.Data;
using DogApp.Models.DTO;
using DogApp.Models;
using DogApp.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DogApp.MSTests
{
    [TestClass]
    public class DogManagerTests
    {
        public enum SortOrder
        {
            Ascending,
            Descending
        }

        [TestMethod]
        public async Task GetDogsAsync_ReturnsDogs()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetDogsTestDatabase")
                .Options;

            List<Dog> dogs = null;

            using (var context = new ApplicationDbContext(options))
            {
                dogs = new List<Dog>
                {
                    new Dog { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                    new Dog { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 }
                };

                context.Dogs.AddRange(dogs);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var dogManager = new DogManager(context);

                // Act
                var result = await dogManager.GetDogsAsync();

                // Assert
                var expectedDogDTOs = dogs.Select(d => new DogDTO
                {
                    Name = d.Name,
                    Color = d.Color,
                    TailLength = d.TailLength,
                    Weight = d.Weight
                }).ToList();

                Assert.AreEqual(expectedDogDTOs.Count, result.Count());

                for (int i = 0; i < expectedDogDTOs.Count; i++)
                {
                    var expected = expectedDogDTOs[i];
                    var actual = result.ElementAt(i);

                    Assert.AreEqual(expected.Name, actual.Name);
                    Assert.AreEqual(expected.Color, actual.Color);
                    Assert.AreEqual(expected.TailLength, actual.TailLength);
                    Assert.AreEqual(expected.Weight, actual.Weight);
                }
            }
        }

        [TestMethod]
        public async Task CreateDogAsync_CreatesDog()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateDogTestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var dogManager = new DogManager(context);

                var newDog = new DogDTO
                {
                    Name = "Buddy",
                    Color = "Brown",
                    TailLength = 10.5,
                    Weight = 25.7
                };

                // Act
                await dogManager.CreateDogAsync(newDog);

                // Assert
                Assert.AreEqual(1, context.Dogs.Count());
            }
        }

        [TestMethod]
        public async Task GetPingAsync_ReturnsPing()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();

                var dogManager = new DogManager(context);

                // Act
                var result = await dogManager.GetPingAsync();

                // Assert
                Assert.AreEqual("Dogshouseservice.Version1.0.1", result);
            }
        }

        [TestMethod]
        public async Task SortByAttributeAsync_SortsDogs()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Dogs.AddRange(new List<Dog>
                {
                    new Dog { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                    new Dog { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 },
                    new Dog { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
                    new Dog { Name = "Max", Color = "Golden", TailLength = 12.0, Weight = 28.8 },
                    new Dog { Name = "Bella", Color = "Black", TailLength = 7.5, Weight = 18.5 },
                    new Dog { Name = "Daisy", Color = "Brown", TailLength = 9.5, Weight = 20.1 }
                });

                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                var dogManager = new DogManager(context);

                var dogs = context.Dogs.Select(d => new DogDTO
                {
                    Name = d.Name,
                    Color = d.Color,
                    TailLength = d.TailLength,
                    Weight = d.Weight
                }).ToList();

                // Act
                var resultAsc = await dogManager.SortByAttributeAsync(dogs, "Name", "asc");
                var resultDesc = await dogManager.SortByAttributeAsync(dogs, "Name", "desc");

                // Assert
                Assert.IsTrue(IsSorted(resultAsc.Select(d => d.Name), SortOrder.Ascending));
                Assert.IsTrue(IsSorted(resultDesc.Select(d => d.Name), SortOrder.Descending));
            }
        }

        [TestMethod]
        public async Task IsDogNameUniqueAsync_ReturnsTrue()
        {
            // Arrange
            var uniqueDogName = "Buddy";

            // Creating a new in-memory database for the test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Dogs.Add(new Dog { Name = "Buddy", Color = "Brown" });
                context.SaveChanges();
            }

            // Creating a real DogManager instance that will use the In-Memory Database
            using (var context = new ApplicationDbContext(options))
            {
                var dogManager = new DogManager(context);

                // Act
                var result = await dogManager.IsDogNameUniqueAsync(uniqueDogName);

                // Assert
                Assert.IsTrue(result);
            }
        }

        private bool IsSorted(IEnumerable<string> values, SortOrder order)
        {
            var sortedValues = order == SortOrder.Ascending
                ? values.OrderBy(v => v)
                : values.OrderByDescending(v => v);

            return values.SequenceEqual(sortedValues);
        }
    }
}
