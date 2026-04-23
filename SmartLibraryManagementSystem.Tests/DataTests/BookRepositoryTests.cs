using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;
using SmartLibraryManagementSystem.Data.Repositories;

namespace SmartLibraryManagementSystem.Tests.DataTests
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<SmartLibraryDbContext> _options;

        public BookRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SmartLibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestLibrary_" + System.Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            // Arrange
            using (var context = new SmartLibraryDbContext(_options))
            {
                context.Books.Add(new Book { Title = "Book 1", Author = "Author 1", BookCode = "B1", Category = "Cat1", ShelfCode = "S1" });
                context.Books.Add(new Book { Title = "Book 2", Author = "Author 2", BookCode = "B2", Category = "Cat2", ShelfCode = "S2" });
                await context.SaveChangesAsync();
            }

            using (var context = new SmartLibraryDbContext(_options))
            {
                var repository = new BookRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectBook()
        {
            // Arrange
            int bookId;
            using (var context = new SmartLibraryDbContext(_options))
            {
                var book = new Book { Title = "Book 1", Author = "Author 1", BookCode = "B1", Category = "Cat1", ShelfCode = "S1" };
                context.Books.Add(book);
                await context.SaveChangesAsync();
                bookId = book.Id;
            }

            using (var context = new SmartLibraryDbContext(_options))
            {
                var repository = new BookRepository(context);

                // Act
                var result = await repository.GetByIdAsync(bookId);

                // Assert
                Assert.Equal("Book 1", result.Title);
            }
        }

        [Fact]
        public async Task AddAsync_AddsBookToDatabase()
        {
            // Arrange
            var book = new Book { Title = "New Book", Author = "Author", BookCode = "NB1", Category = "Cat", ShelfCode = "S1" };

            using (var context = new SmartLibraryDbContext(_options))
            {
                var repository = new BookRepository(context);

                // Act
                await repository.AddAsync(book);
            }

            // Assert
            using (var context = new SmartLibraryDbContext(_options))
            {
                Assert.Equal(1, context.Books.Count());
                Assert.Equal("New Book", context.Books.First().Title);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesBookFromDatabase()
        {
            // Arrange
            int bookId;
            using (var context = new SmartLibraryDbContext(_options))
            {
                var book = new Book { Title = "To Delete", Author = "Author", BookCode = "D1", Category = "Cat", ShelfCode = "S1" };
                context.Books.Add(book);
                await context.SaveChangesAsync();
                bookId = book.Id;
            }

            using (var context = new SmartLibraryDbContext(_options))
            {
                var repository = new BookRepository(context);

                // Act
                await repository.DeleteAsync(bookId);
            }

            // Assert
            using (var context = new SmartLibraryDbContext(_options))
            {
                Assert.Equal(0, context.Books.Count());
            }
        }
    }
}