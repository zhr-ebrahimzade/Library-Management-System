using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Borrower
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required(ErrorMessage = "The First Name field is required.")]
        [StringLength(50, ErrorMessage = "The First Name field cannot exceed 50 characters.")]
        public string?
        FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [StringLength(50, ErrorMessage = "The Last Name field cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        [StringLength(200, ErrorMessage = "The Address field cannot exceed 200 characters.")]
        public string? Address { get; set; }

        //[Required(ErrorMessage = "The Date of Birth field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        // Navigation property
        public ICollection<Loan>? Loans { get; set; }
    }
}
