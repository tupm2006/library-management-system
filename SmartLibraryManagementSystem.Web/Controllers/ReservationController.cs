using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystem.Business.Interfaces;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBook(int studentId, int bookId)
        {
            await _reservationService.ReserveBookAsync(studentId, bookId);
            return Ok();
        }

        public IActionResult MyReservations()
        {
            // Logic to fetch and display reservations for the logged-in student
            return View();
        }
    }
}