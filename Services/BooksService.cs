using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;

namespace Services
{
    public class BooksService : IBooksService
    {
        private readonly LibraryDbContext _dbContext;
        public BooksService(LibraryDbContext libraryDbContext) 
        {
            _dbContext= libraryDbContext;
        }
        public async Task<BookResponse> AddBookAsync(BookAddRequest bookAddRequest)
        {
            //check for null book
            if(bookAddRequest == null) throw new ArgumentNullException(nameof(BookAddRequest));

            //Validate the input data 
            ValidationHelper.Validate(bookAddRequest);

            //check for uniqe ISBN 
            int ISBNCount = await _dbContext.Books.CountAsync(temp => temp.ISBN == bookAddRequest.ISBN);
            if (ISBNCount > 0)
            {
                // Handle duplicate ISBN
                throw new ArgumentException("A book with the same ISBN already exists");
            }


            // Create a new Book entity
            Book newBook = bookAddRequest.ToBook();
            //newBook.ID =     

            //Add newBook to repository
            await _dbContext.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();

            BookResponse bookResponse = newBook.BookToBookResponse();
            return bookResponse;
        }

        public async Task<IEnumerable<BookResponse>> BookSearchByAcync(string field, string searchValue)
        {
            IEnumerable<BookResponse> allBooks = await GetAllBooksAsync();
            //IEnumerable<BookResponse> matchingBooks = allBooks;
            var query = _dbContext.Books.AsQueryable();
            //Check field is not null
            if (!string.IsNullOrEmpty(field) || string.IsNullOrEmpty(searchValue))
            {

                //Get matching book from database based on field and searchValue
                switch (field)
                {
                    case nameof(Book.Title):
                        //matchingBooks = allBooks.Where(b =>
                        //(!string.IsNullOrEmpty(b.Title)) ?
                        //b.Title.Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        query = query.Where(b => (!string.IsNullOrEmpty(b.Title)) ?
                        b.Title.ToLower().Contains(searchValue.ToLower()) : true);
                        break;

                    case nameof(Book.Author.Name):
                        query = query.Where(b => (!string.IsNullOrEmpty(b.Author.Name)) ?
                        b.Author.Name.ToLower().Contains(searchValue.ToLower()) : true);
                        break;

                    case nameof(Book.ISBN):
                        query = query.Where(b => (!string.IsNullOrEmpty(b.ISBN)) ?
                        b.ISBN.ToLower().Contains(searchValue.ToLower()) : true);
                        break;

                    default:
                        //invalid field , return all books
                        break;

                }
            }

            List<Book> result =await query.ToListAsync();

            //Convert Book to Book response

            List<BookResponse> bookResponses = result.Select(b => b.BookToBookResponse()).ToList();
            //return all matching object

            return bookResponses;
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooksAsync()
        {
            IEnumerable<Book> books = await _dbContext.Books.ToListAsync();
            IEnumerable<BookResponse> bookResponses = books.Select(b => b.BookToBookResponse()).ToList();
            return bookResponses;
        }

        public async Task<BookResponse> GetBookByIdAsync(int? bookId)
        {
            //check bookId is not null
            if(bookId == null) return null;
            //Get matching book from database
            Book? book = await _dbContext.Books.FirstOrDefaultAsync(book => book.ID == bookId);

            if (book == null) return null;
            
            BookResponse bookResponse = book.BookToBookResponse();  
            return bookResponse;
        }

        public async Task<bool> DeleteBookAsync(int? bookId)
        {
            //check bookId is not null
            if (bookId == null) throw new ArgumentNullException();
            //get book from database 
            Book? bookFromDB = await _dbContext.Books.FirstOrDefaultAsync(b => b.ID == bookId);
            //Remove book from database 
            if (bookFromDB == null) return false;

                _dbContext.Books.Remove(bookFromDB);
                //save changes
                await _dbContext.SaveChangesAsync();

                return true;
        }


        public async Task<IEnumerable<BookResponse>> GetAvailableBooksAsync()
        {
            //You can customize the filtering criteria and mapping logic based on your specific requirements and entity structure.
            List<Book> books = await _dbContext.Books.Where(b => b.Quantity > 0).ToListAsync();
            List<BookResponse> availableBooks = books.Select(b => b.BookToBookResponse()).ToList();
            return availableBooks;
        }


        public async Task<IEnumerable<BookResponse>?> GetBooksByAuthorAsync(int? authorId)
        {
            //check authorId
            if(authorId == null) return null;

            //get author
            Author? author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.ID == authorId);
            //check if null
            if (author == null) return Enumerable.Empty<BookResponse>();

            //get books by authorId
            List<Book> books = await _dbContext.Books.Where(b => b.AuthorID == authorId).ToListAsync();
            if(books.Count == 0) return Enumerable.Empty<BookResponse>(); 
            List<BookResponse> bookResponses = books.Select(b => b.BookToBookResponse()).ToList();
            return bookResponses;
        }

        public async Task<IEnumerable<BookResponse>> GetMostBorrowedBooksAsync(int count)
        {
            var mostBorrowedBooks = await _dbContext.Loans
               .Include("Book")
               .GroupBy(l => l.Book)
               .OrderByDescending(g => g.Count())
               .Take(count)
               .Select(g => g.Key)
               .ToListAsync();

            List<BookResponse> bookResponses = mostBorrowedBooks.Select(b => b.BookToBookResponse()).ToList();
            return bookResponses;
        }


        public async Task<bool> IsBookAvailableAsync(int? bookId)
        {
            //// Check if there are any active loans for the specified bookId
            //bool isBookAvailable = await _dbContext.Loans
            //    .AnyAsync(l => l.BookID == bookId && l.ReturnDate == null);

            //return !isBookAvailable;
            if (bookId == null) return false;

            Book? book = await _dbContext.Books
             .Where(b => b.Quantity > 0)
            .FirstOrDefaultAsync(b => b.ID == bookId);

            if (book == null)
                return false;
            else return true;
        }



        public Task<IEnumerable<BookResponse>> GetSortedBookAsync(IEnumerable<BookResponse> allBooks, string sortBy, SortOrderOptions sortOrder)
        {
            throw new NotImplementedException();
        }

    }
}