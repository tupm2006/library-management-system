using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Data.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }
}