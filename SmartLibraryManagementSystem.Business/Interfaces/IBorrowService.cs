using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Business.Interfaces
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync();
        Task BorrowBookAsync(int studentId, int bookId);
        Task ReturnBookAsync(int borrowRecordId);
    }
}