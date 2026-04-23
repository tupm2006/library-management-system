using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartLibraryManagementSystem.Business.Interfaces;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;

        public BorrowController(IBorrowService borrowService, IUserService userService, IBookService bookService)
        {
            _borrowService = borrowService;
            _userService = userService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _borrowService.GetAllBorrowRecordsAsync();
            return View(records);
        }

        // Hiện form chọn người mượn và chọn sách
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = new SelectList(await _userService.GetAllUsersAsync(), "Id", "FullName");
            ViewBag.Books = new SelectList(await _bookService.GetAllBooksAsync(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int studentId, int bookId)
        {
            await _borrowService.BorrowBookAsync(studentId, bookId);
            return RedirectToAction(nameof(Index)); // Mượn xong load lại danh sách
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _borrowService.ReturnBookAsync(id);
            return RedirectToAction(nameof(Index)); // Trả xong load lại danh sách
        }
    }
}