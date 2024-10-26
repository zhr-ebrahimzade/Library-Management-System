using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IBooksService
    {
        Task<BookResponse> AddBookAsync(BookAddRequest bookAddRequest);

        Task<BookResponse> GetBookByIdAsync(int? bookId);
        Task<IEnumerable<BookResponse>> GetAllBooksAsync();
        Task<bool> DeleteBookAsync(int? bookId);
        Task<IEnumerable<BookResponse>> BookSearchByAcync(string field, string searchValue);
        Task<IEnumerable<BookResponse>> GetSortedBookAsync(IEnumerable<BookResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        Task<bool> IsBookAvailableAsync(int? bookId);
        Task<IEnumerable<BookResponse>> GetAvailableBooksAsync();
       // Task<bool> ExistsByISBNAsync(string ISBN);
        Task<IEnumerable<BookResponse>?> GetBooksByAuthorAsync(int? authorId);
        Task<IEnumerable<BookResponse>> GetMostBorrowedBooksAsync(int count); 

        //UpdateBook(BookUpdateRequest bookUpdate)

        //GetBookByOtherFields  : for example GetBooksByAuthor(string authorName)

        // Task<IEnumerable<BookResponseDTO>> GetMostBorrowedBooksAsync(int count);
        //Task<BorrowerResponse> GetBorrowerOfBookAsync(int bookId);
        //Task<IEnumerable<LoanResponse>> GetLoanHistoryOfBookAsync(int bookId);
        //Task<IEnumerable<BookResponse>> GetOverdueBooksAsync();
        //Task<IEnumerable<BookResponse>> GetBooksByBorrowerAsync(int borrowerId);
        //Task<BookResponse> BorrowBookAsync(int bookId, int borrowerId);
        //Task<BookResponse> ReturnBookAsync(int bookId);
        //Task<IEnumerable<BookResponse>> GetBooksByAuthorAsync(string authorName);

    }
}
