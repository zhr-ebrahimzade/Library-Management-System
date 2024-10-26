using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class AuthorAddRequest
    {
        [StringLength(50, ErrorMessage = "The Name field cannot exceed 50 characters.")]
        [Required(ErrorMessage = "The Name field is required.")]
        public string? Name { get; set; }

        //[Required(ErrorMessage = "The Birth Date field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        //[Required(ErrorMessage = "The Nationality field is required.")]
        [StringLength(50, ErrorMessage = "The Nationality field cannot exceed 50 characters.")]
        public string? Nationality { get; set; }

        //[Required(ErrorMessage = "The Biography field is required.")]
        [StringLength(500, ErrorMessage = "The Biography field cannot exceed 500 characters.")]
        public string? Biography { get; set; }





        public Author ToAuthor()
        {
            return new Author
            {
                Name = Name,
                BirthDate = BirthDate,
                Nationality = Nationality,
                Biography = Biography
            };
        }
    }
}
