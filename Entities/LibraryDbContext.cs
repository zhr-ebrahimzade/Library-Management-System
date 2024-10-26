using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Borrower> Borrowers { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Borrower>().ToTable("Borrowers");
            modelBuilder.Entity<Loan>().ToTable("Loans");
            modelBuilder.Entity<Author>().ToTable("Authors");

            modelBuilder.Entity<Book>()
            .Property(b => b.ID)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Author>()
            .Property(a => a.ID)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Borrower>()
            .Property(a => a.ID)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Loan>()
            .Property(a => a.ID)
            .ValueGeneratedOnAdd();

            // Book seed data
            modelBuilder.Entity<Book>().HasData(
                new Book { ID = -1, Title = "The Great Gatsby", AuthorID = -1, ISBN = "9780743273565", PublicationDate = new DateTime(1925, 4, 10), NumberOfPages = 218, Quantity = 5 },
                new Book { ID = -2, Title = "To Kill a Mockingbird", AuthorID = -2, ISBN = "9780061120084", PublicationDate = new DateTime(1960, 7, 11), NumberOfPages = 324, Quantity = 3 },
                new Book { ID = -3, Title = "1984", AuthorID = -3, ISBN = "9780451524935", PublicationDate = new DateTime(1949, 6, 8), NumberOfPages = 328, Quantity = 7 },
                new Book { ID = -4, Title = "Pride and Prejudice", AuthorID = -4, ISBN = "9780141439518", PublicationDate = new DateTime(1813, 1, 28), NumberOfPages = 432, Quantity = 10 },
                new Book { ID = -5, Title = "The Catcher in the Rye", AuthorID = -5, ISBN = "9780316769488", PublicationDate = new DateTime(1951, 7, 16), NumberOfPages = 277, Quantity = 2 },
                new Book { ID = -6, Title = "To the Lighthouse", AuthorID = -6, ISBN = "9780156027328", PublicationDate = new DateTime(1927, 5, 5), NumberOfPages = 209, Quantity = 8 }
            );

            // Author seed data
            modelBuilder.Entity<Author>().HasData(
                new Author { ID = -1, Name = "F. Scott Fitzgerald", BirthDate = new DateTime(1896, 9, 24), Nationality = "American", Biography = "American writer known for his novels and stories" },
                new Author { ID = -2, Name = "Harper Lee", BirthDate = new DateTime(1926, 4, 28), Nationality = "American", Biography = "American novelist widely known for 'To Kill a Mockingbird'" },
                new Author { ID = -3, Name = "George Orwell", BirthDate = new DateTime(1903, 6, 25), Nationality = "British", Biography = "English novelist and essayist, known for his dystopian novel '1984'" },
                new Author { ID = -4, Name = "Jane Austen", BirthDate = new DateTime(1775, 12, 16), Nationality = "English", Biography = "English novelist known for her romantic fiction" },
                new Author { ID = -5, Name = "J.D. Salinger", BirthDate = new DateTime(1919, 1, 1), Nationality = "American", Biography = "American writer known for 'The Catcher in the Rye'" },
                new Author { ID = -6, Name = "Virginia Woolf", BirthDate = new DateTime(1882, 1, 25), Nationality = "English", Biography = "English modernist writer and feminist icon" }
            );

            // Borrower seed data
            modelBuilder.Entity<Borrower>().HasData(
                new Borrower { ID = -1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890", Address = "123 Main St", DateOfBirth = new DateTime(1990, 5, 15) },
                new Borrower { ID = -2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "9876543210", Address = "456 Elm St", DateOfBirth = new DateTime(1992, 9, 20) },
                new Borrower { ID = -3, FirstName = "David", LastName = "Johnson", Email = "david@example.com", PhoneNumber = "5555555555", Address = "789 Oak St", DateOfBirth = new DateTime(1985, 3, 10) },
                new Borrower { ID = -4, FirstName = "Sarah", LastName = "Davis", Email = "sarah@example.com", PhoneNumber = "1112223333", Address = "321 Pine St", DateOfBirth = new DateTime(1988, 7, 5) },
                new Borrower { ID = -5, FirstName = "Michael", LastName = "Wilson", Email = "michael@example.com", PhoneNumber = "4445556666", Address = "555 Cedar St", DateOfBirth = new DateTime(1995, 11, 30) },
                new Borrower { ID = -6, FirstName = "Emily", LastName = "Brown", Email = "emily@example.com", PhoneNumber = "7778889999", Address = "999 Walnut St", DateOfBirth = new DateTime(1993, 2, 14) }
            );

            // Loan seed data
            modelBuilder.Entity<Loan>().HasData(
                new Loan { ID = -1, BookID = -1, BorrowerID = -1, LoanDate = new DateTime(2022, 10, 1), ReturnDate = new DateTime(2022, 10, 15) , Active = true },
                new Loan { ID = -2, BookID = -2, BorrowerID = -2, LoanDate = new DateTime(2022, 10, 5), ReturnDate = new DateTime(2022, 10, 20), Active = true },
                new Loan { ID = -3, BookID = -3, BorrowerID = -3, LoanDate = new DateTime(2022, 10, 3), ReturnDate = new DateTime(2022, 10, 17), Active = false },
                new Loan { ID = -4, BookID = -4, BorrowerID = -4, LoanDate = new DateTime(2022, 9, 25), ReturnDate = new DateTime(2022, 10, 10), Active = true },
                new Loan { ID = -5, BookID = -5, BorrowerID = -5, LoanDate = new DateTime(2022, 10, 8), ReturnDate = new DateTime(2022, 10, 22), Active = true },
                new Loan { ID = -6, BookID = -6, BorrowerID = -6, LoanDate = new DateTime(2022, 10, 12), ReturnDate = new DateTime(2022, 10, 27) , Active = true }
            );


        }

    }
}
