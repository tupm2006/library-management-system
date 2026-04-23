// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using SmartLibraryManagementSystem.Business.Interfaces;
// using SmartLibraryManagementSystem.Data.Entities;
// using System.Threading.Tasks;
// using SmartLibraryManagementSystem.Business;
// using SmartLibraryManagementSystem.Business.Services;

// namespace SmartLibraryManagementSystem.Web.Controllers
// {
//     [Authorize(Roles = "Librarian")]
//     public class UsersController : Controller
//     {
//         private readonly IUserService _userService;

//         public UsersController(IUserService userService)
//         {
//             _userService = userService;
//         }

//         public async Task<IActionResult> Index()
//         {
//             var users = await _userService.GetAllUsersAsync();
//             return View(users);
//         }

//         public async Task<IActionResult> Details(int id)
//         {
//             var user = await _userService.GetUserByIdAsync(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//             return View(user);
//         }

//         public IActionResult Create()
//         {
//             return View();
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create(User user)
//         {
//             if (ModelState.IsValid)
//             {
//                 await _userService.AddUserAsync(user);
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(user);
//         }

//         public async Task<IActionResult> Edit(int id)
//         {
//             var user = await _userService.GetUserByIdAsync(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//             return View(user);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(int id, User user)
//         {
//             if (id != user.Id)
//             {
//                 return BadRequest();
//             }

//             if (ModelState.IsValid)
//             {
//                 await _userService.UpdateUserAsync(user);
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(user);
//         }

//         public async Task<IActionResult> Delete(int id)
//         {
//             var user = await _userService.GetUserByIdAsync(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//             return View(user);
//         }

//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(int id)
//         {
//             await _userService.DeleteUserAsync(id);
//             return RedirectToAction(nameof(Index));
//         }
//     }
// }
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystem.Business.Interfaces;
using SmartLibraryManagementSystem.Data.Entities;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // Bỏ qua kiểm tra các danh sách liên kết
            ModelState.Remove("BorrowRecords");
            ModelState.Remove("Reservations");
            ModelState.Remove("Reviews");
            ModelState.Remove("Penalties");

            if (ModelState.IsValid)
            {
                await _userService.AddUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return BadRequest();

            ModelState.Remove("BorrowRecords");
            ModelState.Remove("Reservations");
            ModelState.Remove("Reviews");
            ModelState.Remove("Penalties");

            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}