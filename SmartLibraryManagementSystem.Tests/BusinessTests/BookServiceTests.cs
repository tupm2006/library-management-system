using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SmartLibraryManagementSystem.Business.Interfaces;
using SmartLibraryManagementSystem.Business.Services;
using SmartLibraryManagementSystem.Data.Entities;
using SmartLibraryManagementSystem.Data.Repositories;

namespace SmartLibraryManagementSystem.Tests.BusinessTests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly IBookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2" }
            };
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddBookAsync_CallsRepositoryAddMethod()
        {
            // Arrange
            var book = new Book { Id = 3, Title = "Book 3", Author = "Author 3" };

            // Act
            await _bookService.AddBookAsync(book);

            // Assert
            _mockBookRepository.Verify(repo => repo.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_CallsRepositoryUpdateMethod()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Updated Author" };

            // Act
            await _bookService.UpdateBookAsync(book);

            // Assert
            _mockBookRepository.Verify(repo => repo.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_CallsRepositoryDeleteMethod()
        {
            // Arrange
            var bookId = 1;

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockBookRepository.Verify(repo => repo.DeleteAsync(bookId), Times.Once);
        }
    }
}