using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationContext _db;

        public RatingService(ApplicationContext db)
        {
            _db = db;
        }
        
        public async Task<bool> AddRate(int id, Rating rating)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book != null)
            {
                rating.Book = book;
                var addOrUpdateRating = _db.Ratings.FirstOrDefault(r => r.Book == book);
                if (addOrUpdateRating == null)
                {
                    await _db.Ratings.AddAsync(rating);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    addOrUpdateRating.Score = rating.Score;
                    await _db.SaveChangesAsync();
                }

                return true;
            }
            return false;
        }
    }
}
