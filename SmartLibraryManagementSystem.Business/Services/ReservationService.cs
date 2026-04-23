using System;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;
using SmartLibraryManagementSystem.Data.Repositories;

namespace SmartLibraryManagementSystem.Business.Interfaces
{
    public interface IReservationService
    {
        Task ReserveBookAsync(int studentId, int bookId);
    }

    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;

        public ReservationService(IReservationRepository reservationRepository, IBookRepository bookRepository)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
        }

        public async Task ReserveBookAsync(int studentId, int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.AvailableQuantity <= 0)
            {
                throw new InvalidOperationException("Book is not available for reservation.");
            }

            var reservation = new Reservation
            {
                StudentId = studentId,
                BookId = bookId,
                ReserveDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await _reservationRepository.AddAsync(reservation);
        }
    }
}