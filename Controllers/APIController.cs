using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Task2.Model;

namespace WebAPI_Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public APIController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet("/Get")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Get(string order)
        {
            return await _db.Books
                .OrderBy(book => book.Genre)
                .Select(book => new BookDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Author = book.Author,
                    Rating = _db.Ratings.FirstOrDefault(r => r.BookID == book.ID).Score,
                    ReviewsNumber = _db.Reviews.Where(r => r.BookID == book.ID).Count()
                })
                .ToListAsync();
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<BookDetailsDTO>> Get(int id)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if(book != null)
            {
                return new BookDetailsDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Content = book.Content,
                    Author = book.Author,
                    Rating = _db.Ratings.FirstOrDefault(r => r.BookID == book.ID).Score,
                    Reviews = _db.Reviews.Where(r => r.BookID == book.ID).Select(r => ReviewToDTO(r)).ToList()
                }; 
            }
            return NotFound();
        }

        [HttpPost("/Save")]
        public async Task<IActionResult> Save(Book book)
        {
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/{id}/Review")]
        public async Task<IActionResult> AddReview(int id, Review review)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book != null)
            {
                review.Book = book;
                await _db.Reviews.AddAsync(review);
                await _db.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }

        [HttpPut("/{id}/Rate")]
        public async Task<IActionResult> AddRating(int id, Rating rating)
        {
            if (rating == null || rating.Score > 5) { return BadRequest(); }

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

                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("/{id}/Delete")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public static ReviewDTO ReviewToDTO(Review review) =>
            new ReviewDTO
            {
                ID = review.ID,
                Message = review.Message,
                Reviewer = review.Reviewer
            };
    }
}
