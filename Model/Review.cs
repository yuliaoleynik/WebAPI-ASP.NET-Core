using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Task2.Model
{
    public class Review
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string? Message { get; set; }
        public int? BookID { get; set; }
        [ForeignKey("BookID")]
        public Book? Book { get; set; }
        [Required]
        public string? Reviewer { get; set; }
    }

    public class ReviewDTO
    {
        public int ID { get; set; }
        public string? Message { get; set; }
        public string? Reviewer { get; set; }
    }
}
