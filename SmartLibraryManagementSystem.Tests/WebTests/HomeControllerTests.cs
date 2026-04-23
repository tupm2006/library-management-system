using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartLibraryManagementSystem.Business.Interfaces;
using SmartLibraryManagementSystem.Web.Controllers;
using SmartLibraryManagementSystem.Web.Models;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManagementSystem.Tests.WebTests
{
    public class HomeControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly SmartLibraryDbContext _context;

        public HomeControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            
            var options = new DbContextOptionsBuilder<SmartLibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new SmartLibraryDbContext(options);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(_bookServiceMock.Object, _context);

            // Act
            var result = await controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}