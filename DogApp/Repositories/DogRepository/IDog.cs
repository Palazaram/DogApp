using DogApp.Models;
using DogApp.Models.DTO;

namespace DogApp.Repositories.DogRepository
{
    public interface IDog
    {
        Task<IEnumerable<DogDTO>> GetDogsAsync();
        Task CreateDogAsync(DogDTO dog);
        Task<string> GetPingAsync();
        Task<IEnumerable<DogDTO>> SortByAttributeAsync(IEnumerable<DogDTO> dogs, string attribute, string order);
        Task<bool> IsDogNameUniqueAsync(string name);
    }
}
