using Microsoft.AspNetCore.Mvc;
using WebAPI_Task2.Services;
using WebAPI_Task2.Model;

namespace WebAPI_Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IReviewService reviewService;
        private readonly IRatingService ratingService;

        public BooksController(
            IBookService bookService,
            IReviewService reviewService,
            IRatingService ratingService)
        {
            this.bookService = bookService;
            this.reviewService = reviewService;
            this.ratingService = ratingService;
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
            var book = await bookService.GetOne(id);
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
            int result = await reviewService.AddReview(id, review);
            if (result != 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut("/{id}/Rate")]
        public async Task<ActionResult> AddRating(int id, Rating rating)
        {
            if (rating == null || rating.Score > 5) { return BadRequest(); }

            var result = await ratingService.AddRate(id, rating);

            if (result)
            {
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
        
    }
}
