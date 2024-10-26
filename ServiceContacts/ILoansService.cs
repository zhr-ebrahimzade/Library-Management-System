using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ILoansService
    {
        Task<LoanResponse> BorrowBookAsync(int? bookId, int? borrowerId);

        Task<LoanResponse> ReturnBookAsync(int loanId);

        Task<IEnumerable<LoanResponse>> GetActiveLoansAsync();

        Task<IEnumerable<LoanResponse>> GetOverdueLoansAsync();

        Task<LoanResponse> ExtendLoanAsync(int loanId, int extensionDays);

        Task<int> GetActiveLoanDurationAsync(int loanId);
        Task<int> GetInactiveLoanDurationAsync(int loanId);

        Task<LoanResponse> GetLoanByIdAsync(int? loanId);

        Task<IEnumerable<LoanResponse>> GetLoansByBookAsync(int bookId);

        Task<IEnumerable<LoanResponse>> GetLoansByBorrowerAsync(int borrowerId);

        Task<int> GetTotalLoansCountAsync();
    }
}
