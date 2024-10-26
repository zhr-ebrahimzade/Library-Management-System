using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class BorrowerResponse
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }



        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }


    public static class BorroweExtention
    {
        public static BorrowerResponse ToBorrowerResponse(this Borrower borrower)
        {
            return new BorrowerResponse
            {
                ID = borrower.ID,
                FirstName = borrower.FirstName,
                LastName = borrower.LastName,
                Email = borrower.Email,
                Address = borrower.Address,
                DateOfBirth = borrower.DateOfBirth,
                PhoneNumber = borrower.PhoneNumber,
            };
        }
    }
}
