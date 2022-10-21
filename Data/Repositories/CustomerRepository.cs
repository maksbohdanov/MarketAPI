using System.Collections.Generic;
using Data.Interfaces;
using Data.Entities;
using Data.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TradeMarketDbContext _context;
        public CustomerRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await _context.Customers               
               .Include(x => x.Person)
               .Include(x => x.Receipts)
               .ThenInclude(x => x.ReceiptDetails)               
               .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Customers               
               .Include(x => x.Person)
               .Include(x => x.Receipts)
               .ThenInclude(x => x.ReceiptDetails)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Customer entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
        }

        public void Delete(Customer entity)
        {
            _context.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }        
    }
}
