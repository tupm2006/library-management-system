using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLibraryManagementSystem.Data.Entities
{
    public class BorrowRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public User Student { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public string Status { get; set; } // Borrowing, Returned, Overdue

        // Cột mới để theo dõi việc gia hạn: null (chưa xin), "Pending" (đang chờ duyệt), "Approved" (đã duyệt), "Rejected" (bị từ chối)
        public string ?ExtensionStatus { get; set; }
    }
}