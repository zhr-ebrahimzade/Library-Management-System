using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IBorrowersService
    {

        Task<IEnumerable<BorrowerResponse>> BorrowerSearchByAsync(string field, string searchValue);



        Task<BorrowerResponse> AddBorrowerAsync(BorrowerAddRequest borrowerRequest);

        Task<BorrowerResponse?> GetBorrowerByIdAsync(int? borrowerId);

        Task<IEnumerable<BorrowerResponse>> GetAllBorrowersAsync();

        Task<IEnumerable<BorrowerResponse>> SearchBorrowersAsync(string searchTerm);

        //Task<BorrowerResponse> UpdateBorrowerAsync(int borrowerId, BorrowerUpdateRequest borrowerRequest);

        Task<bool> DeleteBorrowerAsync(int? borrowerId);

        Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(int borrowerId);

        Task<IEnumerable<BorrowerResponse>> GetOverdueBorrowersAsync();

        Task<int> GetBorrowedBooksCountAsync(int borrowerId);

        Task<IEnumerable<LoanResponse>> GetBorrowerLoanHistoryAsync(int borrowerId);
    }
}
