using DogApp.Controllers;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogApp.xUnitTests
{
    public class DogControllerTests
    {
        [Fact]
        public async Task Ping_ShouldReturnPingResponse()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            mockDogService.Setup(service => service.GetPingAsync()).ReturnsAsync("Dogshouseservice.Version1.0.1");
            var controller = new DogController(mockDogService.Object);

            // Act
            var response = await controller.Ping();

            // Assert
            Assert.Equal("Dogshouseservice.Version1.0.1", response);
        }

        [Fact]
        public async Task Dogs_ShouldReturnAllDogs()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Dog1", Color = "Brown" },
                new DogDTO { Name = "Dog2", Color = "Black" },
                new DogDTO { Name = "Dog3", Color = "White" },
            };

            mockDogService.Setup(service => service.GetDogsAsync()).ReturnsAsync(dogs);
            var controller = new DogController(mockDogService.Object);

            // Act
            var result = await controller.Dogs(null, null, null, null) as OkObjectResult;
            var response = result.Value as IEnumerable<DogDTO>;

            // Assert
            Assert.Equal(3, response.Count());
        }

        [Fact]
        public async Task Dog_ShouldCreateDog()
        {
            // Arrange
            var mockDogService = new Mock<IDog>();
            var newDog = new DogDTO
            {
                Name = "Fido",
                Color = "Black",
                Weight = 15.0,
                TailLength = 10.0
            };
            mockDogService.Setup(service => service.CreateDogAsync(newDog)).Returns(Task.CompletedTask);
            var controller = new DogController(mockDogService.Object);

            // Act
            var result = await controller.Dog(newDog) as OkObjectResult;

            // Assert
            Assert.Equal(newDog, result.Value);
        }
    }
}
