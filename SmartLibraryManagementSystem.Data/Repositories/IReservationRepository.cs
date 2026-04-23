using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;

namespace SmartLibraryManagementSystem.Data.Repositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(int id);
    }
}