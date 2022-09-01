using WebAPI_Task2.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebAPI_Task2.Services
{
    public class BookService
    {
        private readonly ApplicationContext _db;

        public BookService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<List<BookDTO>> GetAllBooks(string? order)
        {
            var books =  _db.Books
                .Select(book => new BookDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Author = book.Author,
                    Rating = _db.Ratings.FirstOrDefault(r => r.BookID == book.ID).Score,
                    ReviewsNumber = _db.Reviews.Where(r => r.BookID == book.ID).Count()
                });

            if(order == "title")
            {
                books = books.OrderBy(b => b.Title);
            }
            else if(order == "author")
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
                return await new BookDetailsDTO
                {
                    ID = book.ID,
                    Title = book.Title,
                    Content = book.Content,
                    Author = book.Author,
                    Rating = _db.Ratings.FirstOrDefault(r => r.BookID == book.ID).Score,
                    Reviews = _db.Reviews.Where(r => r.BookID == book.ID).Select(r => ReviewToDTO(r)).ToList()
                };
            }
            else { return null; }
        }

        public async Task<int> AddBook(Book book)
        {
            int bookID = (await _db.Books.AddAsync(book)).Entity.ID;
            await _db.SaveChangesAsync();

            return bookID;
            
        }

        public async Task<bool> UpdateBook(Book book)
        {
            var oldBook = _db.Books.FirstOrDefault(b => b.ID == book.ID);
            if (oldBook is null)
            {
                return false;
            }
            oldBook = book;
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
    }
}
