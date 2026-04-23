using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class StatisticsController : Controller
    {
        private readonly SmartLibraryDbContext _context;

        public StatisticsController(SmartLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Lấy các con số thống kê tổng quan
            ViewBag.TotalTitles = await _context.Books.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync(u => u.Role == "Student");
            ViewBag.TotalBorrowing = await _context.BorrowRecords.CountAsync(br => br.Status == "Borrowing");
            ViewBag.TotalReturned = await _context.BorrowRecords.CountAsync(br => br.Status == "Returned");

            // 2. Lấy danh sách Độc giả mượn quá hạn (Đang mượn VÀ Hạn trả < Ngày hiện tại)
            var overdueRecords = await _context.BorrowRecords
                .Include(br => br.Student)
                .Include(br => br.Book)
                .Where(br => br.Status == "Borrowing" && br.DueDate < DateTime.Now)
                .ToListAsync();

            ViewBag.OverdueRecords = overdueRecords;

            return View();
        }
    }
}