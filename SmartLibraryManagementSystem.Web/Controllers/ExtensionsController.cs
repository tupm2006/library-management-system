using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class ExtensionsController : Controller
    {
        private readonly SmartLibraryDbContext _context;

        public ExtensionsController(SmartLibraryDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách các đơn xin gia hạn
        public async Task<IActionResult> Index()
        {
            var requests = await _context.BorrowRecords
                .Include(b => b.Student)
                .Include(b => b.Book)
                .Where(b => b.ExtensionStatus != null) // Lấy những đơn có dính dáng đến gia hạn
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var record = await _context.BorrowRecords.Include(r => r.Student).FirstOrDefaultAsync(r => r.Id == id);
            if (record != null && record.ExtensionStatus == "Pending")
            {
                record.ExtensionStatus = "Approved";
                record.DueDate = record.DueDate.AddDays(7); // Duyệt gia hạn thêm 7 ngày
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã duyệt gia hạn thêm 7 ngày cho sinh viên {record.Student?.FullName}!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var record = await _context.BorrowRecords.FirstOrDefaultAsync(r => r.Id == id);
            if (record != null && record.ExtensionStatus == "Pending")
            {
                record.ExtensionStatus = "Rejected";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã từ chối yêu cầu gia hạn!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}