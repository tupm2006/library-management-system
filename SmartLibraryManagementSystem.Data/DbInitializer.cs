// using System.Linq;
// using SmartLibraryManagementSystem.Data.Entities;

// namespace SmartLibraryManagementSystem.Data
// {
//     public static class DbInitializer
//     {
//         public static void Initialize(SmartLibraryDbContext context)
//         {
//             context.Database.EnsureCreated();

//             // Check if any users exist
//             if (context.Users.Any())
//             {
//                 return; // Database has been seeded
//             }

//             // Add default librarian
//             var defaultUser = new User
//             {
//                 Username = "admin",
//                 PasswordHash = "123456", // Plaintext for simplicity
//                 FullName = "Default Admin",
//                 Role = "Librarian",
//                 StudentClass = "N/A"
//             };

//             context.Users.Add(defaultUser);
//             context.SaveChanges();
//         }
//     }
// }
using SmartLibraryManagementSystem.Data.Entities;
using System.Linq;

namespace SmartLibraryManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SmartLibraryDbContext context)
        {
            context.Database.EnsureCreated();

            // Tạo tài khoản Admin
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Username = "admin", PasswordHash = "123456", FullName = "Admin Thủ Thư", Role = "Librarian", StudentClass = "N/A" });
                context.SaveChanges();
            }

            // Bơm 4 cuốn sách xịn sò vào Thư viện
            if (!context.Books.Any())
            {
                var books = new Book[]
                {
                    new Book { BookCode = "IT-001", Title = "Clean Code: A Handbook of Agile Software Craftsmanship", Author = "Robert C. Martin", Category = "Software Engineering", ShelfCode = "A1-R1", AvailableQuantity = 5 },
                    new Book { BookCode = "IT-002", Title = "The Pragmatic Programmer: Your Journey to Mastery", Author = "David Thomas, Andrew Hunt", Category = "Programming", ShelfCode = "A1-R2", AvailableQuantity = 3 },
                    new Book { BookCode = "AI-001", Title = "Artificial Intelligence: A Modern Approach", Author = "Stuart Russell, Peter Norvig", Category = "AI & Machine Learning", ShelfCode = "B1-R1", AvailableQuantity = 2 },
                    new Book { BookCode = "DB-001", Title = "Designing Data-Intensive Applications", Author = "Martin Kleppmann", Category = "Database / System Design", ShelfCode = "C2-R3", AvailableQuantity = 4 }
                };
                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}