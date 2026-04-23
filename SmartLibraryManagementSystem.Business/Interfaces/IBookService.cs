using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Business.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}