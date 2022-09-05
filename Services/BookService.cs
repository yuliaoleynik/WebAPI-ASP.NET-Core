using Microsoft.EntityFrameworkCore;
using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationContext _db;

        public BookService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<List<BookDTO>> GetAllBooks(string? order)
        {
            var books = _db.Books
                .Select(book => new BookDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Author = book.Author,
                    Rating = GetScore(_db.Ratings.FirstOrDefault(r => r.BookID == book.ID)),
                    ReviewsNumber = _db.Reviews.Where(r => r.BookID == book.ID).Count()
                });

            if (order == "title")
            {
                books = books.OrderBy(b => b.Title);
            }
            else if (order == "author")
            {
                books = books.OrderBy(b => b.Author);
            }

            return await books.ToListAsync();
        }

        public async Task<BookDetailsDTO?> GetOne(int id)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book != null)
            {
                return new BookDetailsDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Content = book.Content,
                    Author = book.Author,
                    Rating = GetScore(_db.Ratings.FirstOrDefault(r => r.BookID == book.ID)),
                    Reviews = _db.Reviews.Where(r => r.BookID == book.ID).Select(r => ReviewToDTO(r)).ToList()
                };
            }
            else { return null; }
        }

        public async Task<int> AddBook(Book book)
        {
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();

            return _db.Books.Count();

        }

        public async Task<bool> UpdateBook(Book book)
        {
            var oldBook = _db.Books.FirstOrDefault(b => b.ID == book.ID);
            if (oldBook is null)
            {
                return false;
            }

            oldBook.Author = book.Author;
            oldBook.Title = book.Title;
            oldBook.Genre = book.Genre;
            oldBook.Content = book.Content;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            Book? book = _db.Books.FirstOrDefault(b => b.ID == id);
            if (book == null)
            {
                return false;
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }

        public static ReviewDTO ReviewToDTO(Review review) =>
            new ReviewDTO
            {
                ID = review.ID,
                Message = review.Message,
                Reviewer = review.Reviewer
            };

        public static decimal GetScore(Rating? rating)
        {
            if (rating == null)
            {
                return 0;
            }
            return rating.Score;
        }
    }
}
