using WebAPI_Task2.Model;

namespace WebAPI_Task2.Services
{
    public interface IBookService
    {
        public Task<List<BookDTO>> GetAllBooks(string? order);
        public Task<BookDetailsDTO?> GetOne(int id);
        public Task<bool> DeleteBook(int id);
        public Task<int> AddBook(Book book);
        public Task<bool> UpdateBook(Book book);
    }
}
