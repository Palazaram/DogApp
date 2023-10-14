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

namespace DogApp.NUnitTests
{
    [TestFixture]
    public class DogControllerTests
    {
        private ApplicationDbContext _context;
        private DogController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

            _context = new ApplicationDbContext(options);

            _controller = new DogController(new DogManager(_context));
        }

        [Test]
        public async Task Ping_Returns_PingResult()
        {
            // Act
            var result = await _controller.Ping();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual("Dogshouseservice.Version1.0.1", result);
        }

        [Test]
        public async Task Dogs_Returns_AllDogs()
        {
            _context.Dogs.RemoveRange(_context.Dogs);
            _context.SaveChanges();
            // Arrange
            var dog1 = new Dog { Name = "Dog1", Color = "Brown", TailLength = 12.0, Weight = 25.5 };
            var dog2 = new Dog { Name = "Dog2", Color = "Black", TailLength = 10.0, Weight = 20.0 };
            var dog3 = new Dog { Name = "Dog3", Color = "White", TailLength = 15.0, Weight = 18.0 };

            _context.Dogs.Add(dog1);
            _context.Dogs.Add(dog2);
            _context.Dogs.Add(dog3);
            _context.SaveChanges();

            // Act
            var result = await _controller.Dogs(null, null, null, null) as OkObjectResult;
            var resultData = result.Value as IEnumerable<DogDTO>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultData);
            Assert.AreEqual(3, resultData.Count());
        }

        [Test]
        public async Task CreateDog_Returns_Success()
        {
            // Arrange
            var newDog = new DogDTO { Name = "Max", Color = "White", Weight = 10, TailLength = 5 };

            // Act
            IActionResult actionResult = await _controller.Dog(newDog);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult, "The action result should be OkObjectResult");

            OkObjectResult result = (OkObjectResult)actionResult;

            Assert.AreEqual(200, result.StatusCode, "The status code should be 200 (OK)");

            Assert.AreEqual(newDog, result.Value);
        }

        [Test]
        public async Task CreateDog_Returns_BadRequest_IfNameIsNotUnique()
        {
            // Arrange
            var newDog = new DogDTO
            {
                Name = "", 
                Color = "", 
                Weight = 10.5,
                TailLength = 5.7
            };

            // Act
            var result = await _controller.Dog(newDog) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Name and Color must not be empty.", result.Value);
        }

        [Test]
        public async Task Dogs_Returns_SortedAndPagedDogs()
        {
            // Arrange
            _context.Dogs.RemoveRange(_context.Dogs); 

            
            var dog1 = new Dog { Name = "Dog1", Color = "Brown", TailLength = 12.0, Weight = 25.5 };
            var dog2 = new Dog { Name = "Dog2", Color = "Black", TailLength = 10.0, Weight = 20.0 };
            var dog3 = new Dog { Name = "Dog3", Color = "White", TailLength = 15.0, Weight = 18.0 };

            _context.Dogs.Add(dog1);
            _context.Dogs.Add(dog2);
            _context.Dogs.Add(dog3);
            _context.SaveChanges();

            // Act
            var result = await _controller.Dogs("Name", "asc", 1, 2) as OkObjectResult;
            var resultData = result.Value as IEnumerable<DogDTO>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultData);

            
            Assert.AreEqual(2, resultData.Count()); 
            Assert.AreEqual("Dog1", resultData.First().Name);
        }
    }
}
