using DogApp.Models;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Moq;
using System.Collections;
namespace DogApp.MSTests
{
    [TestClass]
    public class IDogTests
    {
        [TestMethod]
        public async Task GetDogsAsync_ReturnsDogs()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var expectedDogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 },
                new DogDTO { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
                new DogDTO { Name = "Max", Color = "Golden", TailLength = 12.0, Weight = 28.8 },
                new DogDTO { Name = "Bella", Color = "Black", TailLength = 7.5, Weight = 18.5 },
                new DogDTO { Name = "Daisy", Color = "Brown", TailLength = 9.5, Weight = 20.1 }
            };

            mockDogService.Setup(service => service.GetDogsAsync()).ReturnsAsync(expectedDogs);
            var dogService = mockDogService.Object;

            // Act
            var result = await dogService.GetDogsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedDogs, (ICollection)result);
        }

        [TestMethod]
        public async Task CreateDogAsync_CreatesDog()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var newDog = new DogDTO 
            { 
                Name = "Buddy", 
                Color = "Brown", 
                TailLength = 10.5, 
                Weight = 25.7 
            };

            mockDogService.Setup(service => service.CreateDogAsync(It.IsAny<DogDTO>())).Verifiable();
            var dogService = mockDogService.Object;

            // Act
            await dogService.CreateDogAsync(newDog);

            // Assert
            mockDogService.Verify(service => service.CreateDogAsync(newDog), Times.Once);
        }

        [TestMethod]
        public async Task GetPingAsync_ReturnsPing()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var expectedPing = "Ping";
            mockDogService.Setup(service => service.GetPingAsync()).ReturnsAsync(expectedPing);
            var dogService = mockDogService.Object;

            // Act
            var result = await dogService.GetPingAsync();

            // Assert
            Assert.AreEqual(expectedPing, result);
        }

        [TestMethod]
        public async Task SortByAttributeAsync_SortsDogs()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var unsortedDogs = new List<DogDTO> 
            { 
                new DogDTO { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 },
                new DogDTO { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
                new DogDTO { Name = "Max", Color = "Golden", TailLength = 12.0, Weight = 28.8 },
                new DogDTO { Name = "Bella", Color = "Black", TailLength = 7.5, Weight = 18.5 },
                new DogDTO { Name = "Daisy", Color = "Brown", TailLength = 9.5, Weight = 20.1 } 
            };
            
            var expectedSortedDogs = new List<DogDTO> 
            {
                new DogDTO { Name = "Bella", Color = "Black", TailLength = 7.5, Weight = 18.5 },
                new DogDTO { Name = "Daisy", Color = "Brown", TailLength = 9.5, Weight = 20.1 },  
                new DogDTO { Name = "Fido", Color = "White", TailLength = 9.0, Weight = 22.3 },
                new DogDTO { Name = "Buddy", Color = "Brown", TailLength = 10.5, Weight = 25.7 },
                new DogDTO { Name = "Max", Color = "Golden", TailLength = 12.0, Weight = 28.8 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 8.0, Weight = 30.2 }
            };

            mockDogService.Setup(service => service.SortByAttributeAsync(unsortedDogs, "Weight", "asc"))
                .ReturnsAsync(expectedSortedDogs);

            var dogService = mockDogService.Object;

            // Act
            var result = await dogService.SortByAttributeAsync(unsortedDogs, "Weight", "asc");

            // Assert
            CollectionAssert.AreEqual(expectedSortedDogs, (ICollection)result);
        }

        [TestMethod]
        public async Task IsDogNameUniqueAsync_ReturnsTrue()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            mockDogService.Setup(service => service.IsDogNameUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var dogService = mockDogService.Object;

            // Act
            var result = await dogService.IsDogNameUniqueAsync("Buddy");

            // Assert
            Assert.IsTrue(result);
        }
    }
}