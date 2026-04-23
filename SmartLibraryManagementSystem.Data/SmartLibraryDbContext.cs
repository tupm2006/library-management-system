using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data.Entities;
using System;

namespace SmartLibraryManagementSystem.Data
{
    public class SmartLibraryDbContext : DbContext
    {
        public SmartLibraryDbContext(DbContextOptions<SmartLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Penalty> Penalties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BorrowRecord>().ToTable("BorrowRecords");
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Penalty>().ToTable("Penalties");

            base.OnModelCreating(modelBuilder);

            // Additional configurations can be added here if needed
        }
    }
}