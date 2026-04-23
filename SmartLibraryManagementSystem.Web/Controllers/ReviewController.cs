using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystem.Business.Interfaces;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int studentId, int bookId, int rating, string comment)
        {
            await _reviewService.AddReviewAsync(studentId, bookId, rating, comment);
            return Ok();
        }
    }
}