using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystem.Data.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        
        public string BookCode { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
        
        public string Category { get; set; }
        
        public int TotalQuantity { get; set; }
        
        public int AvailableQuantity { get; set; }
        
        public string ShelfCode { get; set; }
        

        public ICollection<BorrowRecord> BorrowRecords { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}