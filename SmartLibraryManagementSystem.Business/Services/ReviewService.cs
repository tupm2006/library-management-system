using System;
using System.Threading.Tasks;
using SmartLibraryManagementSystem.Data.Entities;
using SmartLibraryManagementSystem.Data.Repositories;

namespace SmartLibraryManagementSystem.Business.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(int studentId, int bookId, int rating, string comment);
    }

    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(int studentId, int bookId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            }

            var review = new Review
            {
                StudentId = studentId,
                BookId = bookId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
        }
    }
}