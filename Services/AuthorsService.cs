using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly LibraryDbContext _dbContext;
        public AuthorsService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AuthorResponse> AddAuthorAsync(AuthorAddRequest? authorRequest)
        {
            if(authorRequest == null) throw new ArgumentNullException(nameof(authorRequest));

            //validate
            ValidationHelper.Validate(authorRequest);

            //todo : validate bussiness rule

            //Create a new author entity
            Author newAuthor = authorRequest.ToAuthor();

            //Add to repository
            _dbContext.Authors.Add(newAuthor);
            await _dbContext.SaveChangesAsync();

            AuthorResponse authorResponse = newAuthor.ToAuthorResponse();
            return authorResponse;
        }


        public async Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync()
        {
            IEnumerable<Author> authors = await _dbContext.Authors.ToListAsync();
            IEnumerable<AuthorResponse> authorResponses = authors.Select(a => a.ToAuthorResponse());
            return authorResponses;
        }

        public async Task<AuthorResponse?> GetAuthorByIdAsync(int? authorId)
        {
            // Check if authorId is not null
            if (authorId == null)
                return null;

            // Get matching author from database
            Author? author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.ID == authorId);

            if (author == null)
                return null;

            AuthorResponse authorResponse = author.ToAuthorResponse();
            return authorResponse;
        }

        public async Task<bool> DeleteAuthorAsync(int? authorId)
        {
            // Check if authorId is not null
            if (authorId == null)
                throw new ArgumentNullException("AuthorId is null");

            // Get the author from the database
            Author? author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.ID == authorId);

            if (author == null)
                return false;

            // Remove the author from the database
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        //todo : change author to authors 
        public async Task<AuthorResponse?> GetAuthorsOfBookAsync(int? bookId)
        {
            if (bookId == null) throw new ArgumentNullException("Invalid book ID.");

            Book? book = await _dbContext.Books.FirstOrDefaultAsync(b => b.ID == bookId);
            if (book == null) return null;
            Author author  = await _dbContext.Authors.FirstAsync(a => a.ID == book.AuthorID);
            return author.ToAuthorResponse();
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByAuthorAsync(int? authorId)
        {
            if (authorId == null) throw new ArgumentNullException("invalid authorId");

             List<Book> booksByAuthor = 
                await _dbContext.Books
                .Where(b => b.AuthorID == authorId).ToListAsync();

            List<BookResponse> bookResponses = booksByAuthor.Select(b => b.BookToBookResponse()).ToList();
            return bookResponses;
        }

        public async Task<IEnumerable<AuthorResponse>> GetPopularAuthorsAsync(int count)
        {
            // Check if count is valid
            if (count <= 0)
                count = 10;

            // Retrieve popular author IDs based on loan count
            var popularAuthorIds = await _dbContext.Loans.Include("Book")
                .GroupBy(l => l.Book.AuthorID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();

            // Retrieve the popular authors from the database
            var popularAuthors = await _dbContext.Authors
                .Where(a => popularAuthorIds.Contains(a.ID))
                .ToListAsync();

            // Convert the popular authors to author responses
            var authorResponses = popularAuthors.Select(a => a.ToAuthorResponse()).ToList();

            return authorResponses;
        }

        public async Task<IEnumerable<AuthorResponse>> SearchAuthorsAsync(string? searchTerm)
        {
            // Check if the search term is null or empty
            if (string.IsNullOrEmpty(searchTerm))
                throw new ArgumentException("Search term cannot be null or empty.");

            // Retrieve the authors matching the search term from the database
            var matchingAuthors = await _dbContext.Authors
                .Where(a => a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            // Convert the matching authors to author responses
            var authorResponses = matchingAuthors.Select(a => a.ToAuthorResponse()).ToList();

            return authorResponses;
        }
    }
}
