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

namespace DogApp.xUnitTests
{
    public class DogManagerTest
    {
        [Fact]
        public async Task GetDogsAsync_ShouldReturnDogs()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_GetDogsAsync")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);
            var dogManager = new DogManager(dbContext);

            // Adding some sample dogs to the in-memory database
            dbContext.Dogs.Add(new Dog { Name = "Dog1", Color = "Brown" });
            dbContext.Dogs.Add(new Dog { Name = "Dog2", Color = "Black" });
            await dbContext.SaveChangesAsync();

            // Act
            var dogs = await dogManager.GetDogsAsync();

            // Assert
            Assert.NotNull(dogs);
            Assert.Equal(2, dogs.Count());
        }

        [Fact]
        public async Task CreateDogAsync_ShouldCreateDog()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_CreateDogAsync")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);
            var dogManager = new DogManager(dbContext);

            var dogDTO = new DogDTO
            {
                Name = "Fido",
                Color = "Black",
                TailLength = 10.5,
                Weight = 20.2
            };

            // Act
            await dogManager.CreateDogAsync(dogDTO);

            // Assert
            var createdDog = await dbContext.Dogs.FirstOrDefaultAsync();
            Assert.NotNull(createdDog);
            Assert.Equal(dogDTO.Name, createdDog.Name);
        }

        [Fact]
        public async Task GetPingAsync_ShouldReturnVersion()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_GetPingAsync")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);
            var dogManager = new DogManager(dbContext);

            // Act
            var pingResult = await dogManager.GetPingAsync();

            // Assert
            Assert.Equal("Dogshouseservice.Version1.0.1", pingResult);
        }

        [Fact]
        public async Task SortByAttributeAsync_ShouldSortDogs()
        {
            // Arrange
            var dogManager = new DogManager(null);
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Dog3", Color = "Brown", TailLength = 12.0, Weight = 25.5 },
                new DogDTO { Name = "Dog2", Color = "Black", TailLength = 10.0, Weight = 20.0 },
                new DogDTO { Name = "Dog1", Color = "White", TailLength = 15.0, Weight = 18.0 }
            };

            // Act
            var sortedDogsAsc = await dogManager.SortByAttributeAsync(dogs, "Name", "asc");
            var sortedDogsDesc = await dogManager.SortByAttributeAsync(sortedDogsAsc, "TailLength", "asc");

            // Assert
            Assert.Equal("Dog1", sortedDogsAsc.First().Name); 
            Assert.Equal("Dog2", sortedDogsDesc.First().Name); 
        }

        [Fact]
        public async Task IsDogNameUniqueAsync_ShouldReturnTrueForUniqueName()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_IsDogNameUniqueAsync")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);
            var dogManager = new DogManager(dbContext);

            Assert.Empty(dbContext.Dogs);

            // Creating a dog with a unique name
            var uniqueDog = new DogDTO
            {
                Name = "UniqueName",
                Color = "Brown",
                TailLength = 12.0,
                Weight = 25.5
            };

            await dogManager.CreateDogAsync(uniqueDog); 

            // Act
            var isUnique = await dogManager.IsDogNameUniqueAsync("UniqueName");

            // Assert
            Assert.True(isUnique); 
        }
    }
}
