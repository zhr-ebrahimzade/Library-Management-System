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
    public class LoanResponse
    {
        //public int ID { get; set; }

        //[Display(Name = "Book ID")]
        //public int BookID { get; set; }

        //[Display(Name = "Borrower ID")]
        //public int BorrowerID { get; set; }

        //[Display(Name = "Loan Date")]
        //public DateTime LoanDate { get; set; }

        //[Display(Name = "Return Date")]
        //public DateTime? ReturnDate { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Loan? Loan { get; set; }


    }

}
 