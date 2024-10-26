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
    /// DTO class that is used as return type for most of BooksService method
    /// </summary>
    public class BookResponse
    {
        public int ID { get; set; }

        public string? Title { get; set; }

        public int AuthorID { get; set; }

        public string? ISBN { get; set; }

        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Number of Pages")]
        public int NumberOfPages { get; set; }

        public int Quantity { get; set; }

        // public int Count { get; set; } // New property for count

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            BookResponse other = (BookResponse)obj;

            return ID == other.ID &&
                   Title == other.Title &&
                   AuthorID == other.AuthorID &&
                   ISBN == other.ISBN &&
                   PublicationDate == other.PublicationDate &&
                   NumberOfPages == other.NumberOfPages &&
                   Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            // Use a hash code implementation that takes into account the properties used in the Equals method
            return HashCode.Combine(ID, Title, AuthorID, ISBN, PublicationDate, NumberOfPages, Quantity);
        }

    }

    public static class BookExtentions
    {
        public static BookResponse BookToBookResponse(this Book book)
        {
            return new BookResponse
            {
                ID = book.ID,
                Title = book.Title,
                AuthorID = book.AuthorID,
                ISBN = book.ISBN,
                PublicationDate = (DateTime)book.PublicationDate,
                NumberOfPages = book.NumberOfPages,
                Quantity = book.Quantity,

            };
        }
    }
}
