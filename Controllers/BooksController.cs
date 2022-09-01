using Microsoft.AspNetCore.Mvc;
using WebAPI_Task2.Services;
using WebAPI_Task2.Model;

namespace WebAPI_Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService bookService;
        public BooksController(BookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("/Get")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetByOrder(string order)
        {
            var books = await bookService.GetAllBooks(order);
            return Ok(books);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<BookDetailsDTO>> GetByID(int id)
        {
            var book = bookService.GetOne(id);
            if(book is null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost("/Save")]
        public async Task<ActionResult> SaveBook(Book book)
        {
            if (book.ID == 0)
            {
                int bookID = await bookService.AddBook(book);
                return Ok(bookID);
            }
            bool result = await bookService.UpdateBook(book);
            if (result)
            {
                return Ok(book.ID);
            }

            return BadRequest();
        }

        [HttpPut("/{id}/Review")]
        public async Task<ActionResult> SaveReview(int id, Review review)
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
        public async Task<ActionResult> AddRating(int id, Rating rating)
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
        public async Task<ActionResult> DeleteBook(int id)
        {
            bool result = await bookService.DeleteBook(id);
            if (result)
            {
                return Ok();
            }

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
