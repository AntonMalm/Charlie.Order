namespace Charlie.Order.DataAccess.Repositories;

public interface IOrderRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T item);
    Task RemoveAsync(int id);
    Task UpdateAsync(T item);
}