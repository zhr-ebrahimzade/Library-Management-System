using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class AuthorResponse
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }

        [Display(Name = "Biography")]
        public string? Biography { get; set; }
    }
    public static class AuthorExtention
    {
        public static AuthorResponse ToAuthorResponse(this Author author)
        {

            return new AuthorResponse()
            {
                ID = author.ID,
                Name = author.Name,
                BirthDate = author.BirthDate,
                Nationality = author.Nationality,
                Biography = author.Biography,
            };
        }
    }
}
