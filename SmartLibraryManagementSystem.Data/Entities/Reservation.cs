using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLibraryManagementSystem.Data.Entities
{
    public class Reservation
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
        public DateTime ReserveDate { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Fulfilled, Canceled
        public DateTime ReservationDate { get; set; } = DateTime.Now;
    }
}