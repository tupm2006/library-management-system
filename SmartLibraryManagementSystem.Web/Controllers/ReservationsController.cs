using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Librarian")] // Chỉ Thủ thư mới được vào đây
    public class ReservationsController : Controller
    {
        private readonly SmartLibraryDbContext _context;

        public ReservationsController(SmartLibraryDbContext context)
        {
            _context = context;
        }

        // Hiện danh sách tất cả các đơn đặt trước
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Student)
                .Include(r => r.Book)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }

        // Hàm Duyệt đơn: Đổi trạng thái và tự động tạo Phiếu mượn
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var res = await _context.Reservations
                .Include(r => r.Book)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (res != null && res.Status == "Pending")
            {
                res.Status = "Completed"; // Đánh dấu đã nhận sách

                // Tự động sinh ra 1 phiếu mượn chính thức
                var borrow = new BorrowRecord
                {
                    StudentId = res.StudentId,
                    BookId = res.BookId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14), // Mặc định cho mượn 14 ngày
                    Status = "Borrowing"
                };
                
                _context.BorrowRecords.Add(borrow);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Đã giao sách '{res.Book?.Title}' cho sinh viên '{res.Student?.FullName}' thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Hàm Hủy đơn thủ công: Hủy và trả lại sách lên hệ thống
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var res = await _context.Reservations.Include(r => r.Book).FirstOrDefaultAsync(r => r.Id == id);
            if (res != null && res.Status == "Pending")
            {
                res.Status = "Cancelled";
                if (res.Book != null)
                {
                    res.Book.AvailableQuantity += 1; // Cộng trả lại sách lên giá
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã hủy đơn đặt sách và hoàn trả số lượng vào kho!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}