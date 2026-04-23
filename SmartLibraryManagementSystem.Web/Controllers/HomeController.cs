using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Business.Interfaces;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly SmartLibraryDbContext _context; // Gọi thẳng DB để xử lý comment cho nhanh

        public HomeController(IBookService bookService, SmartLibraryDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        // public async Task<IActionResult> Index()
        // {
        //     var books = await _bookService.GetAllBooksAsync();
        //     return View(books);
        // }
        // 1. HÀM TRANG CHỦ MỚI (CÓ TÌM KIẾM & TỰ ĐỘNG DỌN DẸP SAU 24H)
        public async Task<IActionResult> Index(string searchString)
        {
            // --- TUYỆT CHIÊU LAZY CLEANUP: Hủy đơn quá 24h ---
            // Tìm các đơn đang Pending và đã qua 24 tiếng
            var expiredReservations = await _context.Reservations
                .Include(r => r.Book)
                .Where(r => r.Status == "Pending" && r.ReservationDate.AddHours(24) < System.DateTime.Now)
                .ToListAsync();

            if (expiredReservations.Any())
            {
                foreach (var res in expiredReservations)
                {
                    res.Status = "Expired"; // Đánh dấu là hết hạn
                    if (res.Book != null) 
                    {
                        res.Book.AvailableQuantity += 1; // Cộng trả lại 1 cuốn lên giá
                    }
                }
                await _context.SaveChangesAsync(); // Lưu đồng loạt
            }
            // --------------------------------------------------

            // --- TÌM KIẾM SÁCH ---
            var books = await _context.Books.ToListAsync();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                // Lọc theo tên sách hoặc tên tác giả (Không phân biệt hoa thường)
                books = books.Where(b => 
                    (b.Title != null && b.Title.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)) || 
                    (b.Author != null && b.Author.Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            ViewBag.CurrentSearch = searchString; // Giữ lại từ khóa trên ô tìm kiếm
            return View(books);
        }

        // 2. HÀM XỬ LÝ KHI BẤM NÚT "ĐĂNG KÝ MƯỢN NGAY"
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ReserveBook(int bookId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity!.Name);
            var book = await _context.Books.FindAsync(bookId);

            if (currentUser != null && book != null && book.AvailableQuantity > 0)
            {
                var existing = await _context.Reservations
                    .FirstOrDefaultAsync(r => r.StudentId == currentUser.Id && r.BookId == bookId && r.Status == "Pending");

                if (existing == null)
                {
                    book.AvailableQuantity -= 1; 
                    var reservation = new Reservation
                    {
                        StudentId = currentUser.Id,
                        BookId = bookId,
                        Status = "Pending",
                        ReservationDate = System.DateTime.Now 
                    };
                    
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                    
                    // Bắn thông báo THÀNH CÔNG ra màn hình
                    TempData["SuccessMessage"] = $"Đã đặt trước cuốn '{book.Title}'! Vui lòng đến Kiosk thư viện nhận sách trong vòng 24h.";
                }
                else
                {
                    // Bắn thông báo LỖI (nếu cố tình spam click)
                    TempData["ErrorMessage"] = $"Bạn đã đặt cuốn sách này rồi, đang chờ lên thư viện lấy sách!";
                }
            }
            return RedirectToAction("Index");
        }
        // --- TÍNH NĂNG MỚI: TỦ SÁCH CÁ NHÂN CỦA SINH VIÊN ---
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyBooks()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity!.Name);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            // 1. Lấy sách đang ĐẶT TRƯỚC (chưa lấy)
            ViewBag.MyReservations = await _context.Reservations
                .Include(r => r.Book)
                .Where(r => r.StudentId == currentUser.Id && r.Status == "Pending")
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            // 2. Lấy LỊCH SỬ MƯỢN / TRẢ
            var myBorrows = await _context.BorrowRecords
                .Include(b => b.Book)
                .Where(b => b.StudentId == currentUser.Id)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return View(myBorrows);
        }

        public async Task<IActionResult> HotBooks(string filter = "month")
        {
            var books = await _bookService.GetAllBooksAsync();
            ViewBag.CurrentFilter = filter;
            return View(books);
        }

        // --- TÍNH NĂNG MỚI: CHI TIẾT SÁCH & BÌNH LUẬN ---
        public async Task<IActionResult> BookDetails(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            // 1. Lấy danh sách bình luận của sách này
            ViewBag.Reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.BookId == id)
                .OrderByDescending(r => r.Id) // Mới nhất lên đầu
                .ToListAsync();

            // 2. Kiểm tra xem User hiện tại đã mượn sách này chưa (để cho phép Đánh giá Sao)
            bool hasBorrowed = false;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);
                if (currentUser != null)
                {
                    hasBorrowed = await _context.BorrowRecords
                        .AnyAsync(br => br.BookId == id && br.StudentId == currentUser.Id);
                }
            }
            ViewBag.HasBorrowed = hasBorrowed;

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(int bookId, string commentText, int rating = 0)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity!.Name);
            if (currentUser != null && !string.IsNullOrEmpty(commentText))
            {
                // Nếu có rating (nghĩa là người dùng đã mượn và đánh giá), ta gắn số Sao vào đầu comment
                string finalComment = rating > 0 ? $"[Đánh giá {rating} Sao] {commentText}" : commentText;

                var review = new Review
                {
                    BookId = bookId,
                    StudentId = currentUser.Id,
                    Comment = finalComment
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }

            // Xong thì quay lại đúng trang chi tiết của cuốn sách đó
            return RedirectToAction("BookDetails", new { id = bookId });
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RequestExtension(int borrowId)
        {
            var record = await _context.BorrowRecords.FindAsync(borrowId);
            // Chỉ cho phép gia hạn nếu sách đang mượn và chưa xin gia hạn lần nào
            if (record != null && record.Status == "Borrowing" && string.IsNullOrEmpty(record.ExtensionStatus))
            {
                record.ExtensionStatus = "Pending";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã gửi yêu cầu gia hạn! Vui lòng chờ Thủ thư duyệt.";
            }
            return RedirectToAction("MyBooks");
        }
    }
}