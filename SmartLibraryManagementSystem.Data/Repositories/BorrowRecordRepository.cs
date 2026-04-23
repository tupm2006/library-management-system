using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Data.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly SmartLibraryDbContext _context;

        public BorrowRecordRepository(SmartLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        {
            return await _context.BorrowRecords.ToListAsync();
        }

        public async Task<BorrowRecord> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords.FindAsync(id);
        }

        public async Task AddAsync(BorrowRecord borrowRecord)
        {
            await _context.BorrowRecords.AddAsync(borrowRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BorrowRecord borrowRecord)
        {
            _context.BorrowRecords.Update(borrowRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);
            if (borrowRecord != null)
            {
                _context.BorrowRecords.Remove(borrowRecord);
                await _context.SaveChangesAsync();
            }
        }
    }
}