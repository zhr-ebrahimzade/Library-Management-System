using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class LoanAddRequest
    {
        [Required(ErrorMessage = "Loan date is required.")]
        public DateTime LoanDate { get; set; }

        [Required(ErrorMessage = "Return date is required.")]
        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Book is required.")]
        public int BookID { get; set; }
        //public Book Book { get; set; }

        [Required(ErrorMessage = "Borrower is required.")]
        public int BorrowerID { get; set; }



        // Navigation properties
        //[ForeignKey("BookID")]
        //public Book? Book { get; set; }
        //[ForeignKey("BorrowerID")]
        //public Borrower? Borrower { get; set; }

        public Loan ToLoan()
        {
            return new Loan() 
            {
                BookID = BookID,
                LoanDate= LoanDate,
                ReturnDate= ReturnDate,
                BorrowerID= BorrowerID
            };
        }
    }
}
