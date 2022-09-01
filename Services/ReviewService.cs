using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public class ReviewService
    {
        private readonly ApplicationContext _db;

        public ReviewService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<int> AddReview(int id, Review review)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book != null)
            {
                review.Book = book;
                int reviewID = (await _db.Reviews.AddAsync(review)).Entity.ID;
                await _db.SaveChangesAsync();

                return reviewID;
            }
            return 0;
        }
    }
}
