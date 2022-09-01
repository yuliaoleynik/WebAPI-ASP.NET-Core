using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public interface IReviewService
    {
        public Task<int> AddReview(int id, Review review);
    }
}
