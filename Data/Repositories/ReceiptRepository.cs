using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly TradeMarketDbContext _context;
        public ReceiptRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await _context.Receipts.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await _context.Receipts
               .Include(x => x.Customer)
               .ThenInclude(x => x.Person)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category)
               .ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await _context.Receipts.FindAsync(id);
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Receipts
               .Include(x => x.Customer)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Receipt entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task AddAsync(Receipt entity)
        {
            await _context.Receipts.AddAsync(entity);
        }

        public void Delete(Receipt entity)
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
