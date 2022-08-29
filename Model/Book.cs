using System.ComponentModel.DataAnnotations;

namespace WebAPI_Task2.Model
{
    public class Book
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Не указан контент")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "Не указан автор")]
        public string? Author { get; set; }
        [Required(ErrorMessage = "Не указан жанр")]
        public string? Genre { get; set; }
    }

    public class BookDTO
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? Rating { get; set; }
        public int ReviewsNumber { get; set; }
    }

    public class BookDetailsDTO
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public decimal? Rating { get; set; }
        public List<ReviewDTO>? Reviews { get; set; }
    }
}
