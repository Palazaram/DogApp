using DogApp.Controllers;
using DogApp.Data;
using DogApp.Models;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogApp.MSTests
{
    [TestClass]
    public class DogControllerTests
    {
        [TestMethod]
        public async Task Ping_ReturnsPingString()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            dogService.Setup(d => d.GetPingAsync()).ReturnsAsync("Dogshouseservice.Version1.0.1");
            var controller = new DogController(dogService.Object);

            // Act
            var result = await controller.Ping();

            // Assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Dogshouseservice.Version1.0.1", result);
        }

        [TestMethod]
        public async Task Dogs_ReturnsAllDogs()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Buddy", Color = "Brown" },
                new DogDTO { Name = "Rex", Color = "Black" },
            };

            dogService.Setup(d => d.GetDogsAsync()).ReturnsAsync(dogs);
            var controller = new DogController(dogService.Object);

            // Act
            var result = await controller.Dogs(null, null, null, null) as OkObjectResult;
            var resultData = result.Value as IEnumerable<DogDTO>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultData, typeof(IEnumerable<DogDTO>));
            Assert.AreEqual(2, resultData.Count());
        }

        [TestMethod]
        public async Task Dogs_ReturnsSortedDogs()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Dogs.Add(new Dog { Name = "Buddy", Color = "Brown" });
                context.Dogs.Add(new Dog { Name = "Rex", Color = "Black" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                var dogService = new DogManager(context); 
                var controller = new DogController(dogService);

                // Act
                var result = await controller.Dogs("Name", "asc", null, null) as OkObjectResult;
                var resultData = result.Value as IEnumerable<DogDTO>;

                // Assert
                Assert.IsNotNull(resultData);
                Assert.AreEqual(2, resultData.Count());
                Assert.AreEqual("Buddy", resultData.First().Name);
            }
        }

        [TestMethod]
        public async Task Dog_ReturnsBadRequestForInvalidModel()
        {
            // Arrange
            var dogService = new Mock<IDog>();
            var controller = new DogController(dogService.Object);
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var result = await controller.Dog(new DogDTO()) as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
