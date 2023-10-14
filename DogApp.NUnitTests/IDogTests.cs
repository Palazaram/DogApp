using DogApp.Data;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogApp.NUnitTests
{
    [TestFixture]
    public class IDogTests
    {
        [Test]
        public async Task GetDogsAsync_ReturnsDogs()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var expectedDogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown" },
                new DogDTO { Name = "Rex", Color = "Black" },
            };
            dogService.Setup(d => d.GetDogsAsync()).ReturnsAsync(expectedDogs);

            // Act
            var result = await dogService.Object.GetDogsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedDogs, result.ToList());
        }

        [Test]
        public async Task CreateDogAsync_CreatesDog()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetDogsTestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var dogService = new DogManager(context);

                var newDog = new DogDTO
                {
                    Name = "Buddy",
                    Color = "Brown",
                    TailLength = 10.5,
                    Weight = 25.7
                };

                // Act
                await dogService.CreateDogAsync(newDog);

                // Assert
                var result = await dogService.GetDogsAsync();
                Assert.AreEqual(1, result.Count(d => d.Name == newDog.Name));
            }
        }

        [Test]
        public async Task GetPingAsync_ReturnsPing()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var expectedPing = "Dogshouseservice.Version1.0.1";
            dogService.Setup(d => d.GetPingAsync()).ReturnsAsync(expectedPing);

            // Act
            var result = await dogService.Object.GetPingAsync();

            // Assert
            Assert.AreEqual(expectedPing, result);
        }

        [Test]
        public async Task SortByAttributeAsync_SortsDogs()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown" },
                new DogDTO { Name = "Rex", Color = "Black" },
            };

            dogService.Setup(d => d.SortByAttributeAsync(dogs, "Name", "asc")).ReturnsAsync(dogs.OrderBy(d => d.Name));
            dogService.Setup(d => d.SortByAttributeAsync(dogs, "Name", "desc")).ReturnsAsync(dogs.OrderByDescending(d => d.Name));

            // Act
            var resultAsc = await dogService.Object.SortByAttributeAsync(dogs, "Name", "asc");
            var resultDesc = await dogService.Object.SortByAttributeAsync(dogs, "Name", "desc");

            // Assert
            CollectionAssert.AreEqual(dogs.OrderBy(d => d.Name), resultAsc.ToList());
            CollectionAssert.AreEqual(dogs.OrderByDescending(d => d.Name), resultDesc.ToList());
        }

        [Test]
        public async Task IsDogNameUniqueAsync_ReturnsTrue()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var existingDogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown" },
                new DogDTO { Name = "Rex", Color = "Black" },
            };
            dogService.Setup(d => d.GetDogsAsync()).ReturnsAsync(existingDogs);
    
            dogService.Setup(d => d.IsDogNameUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => !existingDogs.Any(dog => dog.Name == name));

            // Act
            var isUnique = await dogService.Object.IsDogNameUniqueAsync("Fido");

            // Assert
            Assert.IsTrue(isUnique);
        }
    }
}
