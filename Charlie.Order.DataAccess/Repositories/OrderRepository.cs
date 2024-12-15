
using Charlie.Order.DataAccess.DataModels;

namespace Charlie.Order.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository<OrderModel>
    {
        public Task AddAsync(OrderModel entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
