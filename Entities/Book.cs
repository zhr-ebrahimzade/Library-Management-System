using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Book
    {
        //Book Entity:

        //ID(int)
        //Title(string)
        //Author(string)
        //ISBN(string)
        //PublicationDate(DateTime)
        //NumberOfPages(int)
        //Quantity(int)
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The AutorID field is required.")]
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "The ISBN field is required.")]
        [RegularExpression(@"\b\d{9}(\d|X)\b", ErrorMessage = "Invalid ISBN format. The ISBN should be a 10-digit or 13-digit code.")]
        public string? ISBN { get; set; }

        //[Required(ErrorMessage = "The Publication Date field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PublicationDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Number of Pages field must be a positive integer.")]
        public int NumberOfPages { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Quantity field must be a non-negative integer.")]
        public int Quantity { get; set; }



        // Navigation property
        [ForeignKey("AuthorID")]
        public Author? Author { get; set; }

    }


}