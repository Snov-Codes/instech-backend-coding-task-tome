using Domain.Entities;

namespace Persistance.Interfaces
{
    public interface ICosmosDbRepository<T> where T : BaseEntity
    {
        Task AddItemAsync(T item);
        Task DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(string queryString = "SELECT * FROM c");
    }
}
