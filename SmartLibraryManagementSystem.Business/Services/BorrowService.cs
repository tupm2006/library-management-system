using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Business.Interfaces;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Business.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly SmartLibraryDbContext _context;

        public BorrowService(SmartLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            // Lấy kèm thông tin tên Sinh viên và tên Sách để hiển thị
            return await _context.BorrowRecords
                .Include(b => b.Student)
                .Include(b => b.Book)
                .ToListAsync();
        }

        public async Task BorrowBookAsync(int studentId, int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null && book.AvailableQuantity > 0)
            {
                book.AvailableQuantity -= 1; // Tự động trừ số lượng sách
                var record = new BorrowRecord
                {
                    StudentId = studentId,
                    BookId = bookId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14), // Cho mượn 14 ngày
                    Status = "Borrowing"
                };
                _context.BorrowRecords.Add(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReturnBookAsync(int borrowRecordId)
        {
            var record = await _context.BorrowRecords.Include(b => b.Book).FirstOrDefaultAsync(r => r.Id == borrowRecordId);
            if (record != null && record.Status == "Borrowing")
            {
                record.Status = "Returned";
                record.ReturnDate = DateTime.Now;
                if (record.Book != null)
                {
                    record.Book.AvailableQuantity += 1; // Tự động cộng lại sách vào kho
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}   