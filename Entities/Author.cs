using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Author
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [StringLength(100 , ErrorMessage = "The Name field cannot exceed 50 characters.")]
        [Required(ErrorMessage = "The Name field is required.")]
        public string? Name { get; set; }

        //[Required(ErrorMessage = "The Birth Date field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        //[Required(ErrorMessage = "The Nationality field is required.")]
        [StringLength(100 , ErrorMessage = "The Nationality field cannot exceed 50 characters.")]
        public string? Nationality { get; set; }

        //[Required(ErrorMessage = "The Biography field is required.")]
        [StringLength(500, ErrorMessage = "The Biography field cannot exceed 500 characters.")]
        public string? Biography { get; set; }


        // Navigation property
        [InverseProperty("Author")] // Specify the navigation property to which it is inverse
        public ICollection<Book>? Books { get; set; }
    }
}
