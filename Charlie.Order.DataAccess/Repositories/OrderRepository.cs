
using Charlie.Order.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Charlie.Order.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository<OrderModel>
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderModel item)
        {
            _context.Order.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await _context.Order.ToListAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            return await _context.Order.FindAsync(id);
        }

        public async Task RemoveAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            _context.Order.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderModel item)
        {
            _context.Order.Update(item);
            await _context.SaveChangesAsync();

        }
    }
}
