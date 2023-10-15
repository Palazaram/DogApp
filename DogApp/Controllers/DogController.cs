using DogApp.Filters;
using DogApp.Models.DTO;
using DogApp.Repositories.DogRepository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DogApp.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class DogController : ControllerBase
    {
        private readonly IDog _IDog;

        public DogController(IDog iDog)
        {
            _IDog = iDog;
        }

        // Get Ping
        [HttpGet]
        public async Task<string> Ping()
        {
            return await _IDog.GetPingAsync();
        }

        // Get all dogs, sorting, pagination
        [HttpGet]
        public async Task<IActionResult> Dogs([FromQuery] string? attribute, [FromQuery] string? order, 
            [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                IEnumerable<DogDTO> dogs = await _IDog.GetDogsAsync();

                if (!string.IsNullOrWhiteSpace(attribute) && !string.IsNullOrWhiteSpace(order))
                {
                    // If there are parameters, then sort
                    dogs = await _IDog.SortByAttributeAsync(dogs, attribute, order);
                }

                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    // If there are parameters pageNumber & pageSize, then do pagination
                    var pagedDogs = dogs.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
                    return Ok(pagedDogs);
                }
                else if (!pageNumber.HasValue && pageSize.HasValue) 
                {
                    var pagedDogs = dogs.Take(pageSize.Value);
                    return Ok(pagedDogs);
                }
                else
                {
                    // If there are no pagination options, return all dogs
                    return Ok(dogs);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Create a dog
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Dog([FromBody] DogDTO dog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (dog == null)
                    {
                        throw new NullReferenceException("Invalid data.");
                    }

                    if (string.IsNullOrWhiteSpace(dog.Name) || string.IsNullOrWhiteSpace(dog.Color))
                    {
                        return BadRequest("Name and Color must not be empty.");
                    }

                    if (!await _IDog.IsDogNameUniqueAsync(dog.Name))
                    {
                        if (double.TryParse(dog.Weight.ToString(), out double weight) && double.TryParse(dog.TailLength.ToString(), out double tailLength))
                        {
                            if (double.IsNegative(tailLength) && (double.IsNegative(weight) || weight == 0))
                            {
                                throw new ArgumentException("Tail length can't be negative. Weight can't be negative or 0.");
                            }
                            else if (double.IsNegative(tailLength))
                            {
                                throw new ArgumentException("Tail length can't be negative.");
                            }
                            else if (double.IsNegative(weight))
                            {
                                throw new ArgumentException("Weight can't be negative.");
                            }
                            else if (weight == 0)
                            {
                                throw new ArgumentException("Weight can't be 0.");
                            }
                            else
                            {
                                await _IDog.CreateDogAsync(dog);
                                return Ok(dog);
                            }
                        }
                        else
                        {
                            return BadRequest("Weight and TailLength must be valid numeric values.");
                        }
                    }
                    else
                    {
                        return BadRequest("Dog with this name already exists.");
                    }
                }
                else
                {
                    var errors = ModelState
                                .Where(x => x.Value.Errors.Any())
                                .ToDictionary(
                                    x => x.Key,
                                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToList());

                    return BadRequest(new { errors });
                }
            }
            catch (JsonException ex)
            {
                return BadRequest("Invalid JSON data. " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
