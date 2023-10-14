using DogApp.Data;
using DogApp.Models;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DogApp.xUnitTests
{
    public class IDogTests
    {
        [Fact]
        public async Task GetDogsAsync_ShouldReturnDogs()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            dogService.Setup(s => s.GetDogsAsync()).ReturnsAsync(new List<DogDTO>());

            // Act
            var result = await dogService.Object.GetDogsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateDogAsync_ShouldCreateDog()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var dbContext = new ApplicationDbContext(dbContextOptions); // Creating a fake DbContext
            var dogService = new DogManager(dbContext);
            var dogDTO = new DogDTO
            {
                Name = "Fido",
                Color = "Black"
            };

            // Act
            await dogService.CreateDogAsync(dogDTO);

            // Assert
            var createdDog = await dbContext.Dogs.FirstOrDefaultAsync();

            Assert.NotNull(createdDog);
            Assert.NotEqual(0, createdDog.Id);
            Assert.Equal(dogDTO.Name, createdDog.Name);
        }

        [Fact]
        public async Task GetPingAsync_ShouldReturnPing()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            dogService.Setup(s => s.GetPingAsync()).ReturnsAsync("Dogshouseservice.Version1.0.1");

            // Act
            var result = await dogService.Object.GetPingAsync();

            // Assert
            Assert.Equal("Dogshouseservice.Version1.0.1", result);
        }

        [Fact]
        public async Task SortByAttributeAsync_ShouldSortDogs()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Red" },
                new DogDTO { Name = "Max", Color = "Black" },
                new DogDTO { Name = "Charlie", Color = "Gold" }
            };

            dogService.Setup(s => s.SortByAttributeAsync(It.IsAny<IEnumerable<DogDTO>>(), "Name", "asc"))
                .ReturnsAsync(dogs.OrderBy(dog => dog.Name));

            // Act
            var result = await dogService.Object.SortByAttributeAsync(dogs, "Name", "asc");

            // Assert
            Assert.Collection(result,
                dog => Assert.Equal("Buddy", dog.Name),
                dog => Assert.Equal("Charlie", dog.Name),
                dog => Assert.Equal("Max", dog.Name)
            );
        }

        [Fact]
        public async Task IsDogNameUniqueAsync_ShouldCheckNameUniqueness()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var existingDogName = "Buddy";
            dogService.Setup(s => s.IsDogNameUniqueAsync(existingDogName)).ReturnsAsync(false);

            // Act
            var result = await dogService.Object.IsDogNameUniqueAsync(existingDogName);

            // Assert
            Assert.False(result);
        }
    }
}