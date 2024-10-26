using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Loan
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loan date is required.")]
        public DateTime LoanDate { get; set; }

        [Required(ErrorMessage = "Return date is required.")]
        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Book is required.")]
        public int BookID { get; set; }
        //public Book Book { get; set; }

        [Required(ErrorMessage = "Borrower is required.")]
        public int BorrowerID { get; set; }
        public bool Active { get; set; } // Indicates if the book is still on loan



        // Navigation properties
        [ForeignKey("BookID")]
        public Book? Book { get; set; }
        [ForeignKey("BorrowerID")]
        public Borrower? Borrower { get; set; }
    }
}
