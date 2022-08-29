using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Task2.Model
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }
        public int? BookID { get; set; }
        [ForeignKey("BookID")]
        public Book? Book { get; set; }
        public decimal Score { get; set; }
    }
}
