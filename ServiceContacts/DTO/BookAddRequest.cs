using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new book 
    /// </summary>
    public class BookAddRequest
    {
        [Required(ErrorMessage = "The Title field is required.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "The Author Id is required ")]
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "The ISBN field is required.")]
        [RegularExpression(@"\b\d{9}(\d|X)\b", ErrorMessage = "Invalid ISBN format. The ISBN should be a 10-digit or 13-digit code.")]
        public string? ISBN { get; set; }

        //[Required(ErrorMessage = "The Publication Date field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PublicationDate { get; set; }

        [Range(1, 10000, ErrorMessage = "The Number of Pages field must be a positive integer.")]
        public int NumberOfPages { get; set; }

        [Range(0, 1000, ErrorMessage = "The Quantity field must be a non-negative integer.")]
        public int Quantity { get; set; }

        public Book ToBook()
        {
            return new Book()
            {
                Title= Title,
                AuthorID= AuthorID,
                ISBN= ISBN,
                PublicationDate= PublicationDate,
                NumberOfPages= NumberOfPages,
                Quantity=Quantity
            };
        }
    }
}
