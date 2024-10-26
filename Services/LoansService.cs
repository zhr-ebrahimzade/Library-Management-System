using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LoansService : ILoansService
    {
        private readonly LibraryDbContext _dbContext;
        public LoansService(LibraryDbContext libraryDbContext)
        {
            _dbContext = libraryDbContext;
        }
        public async Task<LoanResponse> BorrowBookAsync(int? bookId, int? borrowerId)
        {
            //check borrowerId and bookId
            if (bookId == null || borrowerId == null)
            {
                LoanResponse nullInputLoan = new LoanResponse { Success = false, Message = "Invalid book ID or borrower ID." };
                return await Task.FromResult(nullInputLoan);
            }

            // Check if the book exists
            Book book = await _dbContext.Books.FirstOrDefaultAsync(b => b.ID == bookId);
            if (book == null)
            {
                LoanResponse nullBookLoan = new LoanResponse { Success = false, Message = "Book not found." };
                return await Task.FromResult(nullBookLoan);
            }

            // Check if the book is available for borrowing
            if (book.Quantity <= 0)
            {
                LoanResponse endQuantityLoan = new LoanResponse { Success = false, Message = "Book is not available for borrowing." };
                return await Task.FromResult(endQuantityLoan);
            }

            // Check if the borrower exists
            Borrower borrower = await _dbContext.Borrowers.FirstOrDefaultAsync(b => b.ID == borrowerId);
            if (borrower == null)
            {
                LoanResponse nullBorrowerLoan = new LoanResponse { Success = false, Message = "Borrower not found." };
                return await Task.FromResult(nullBorrowerLoan);

            }
            //create loan 
            Loan loan = new Loan()
            {
                BookID = (int)bookId,
                BorrowerID = (int)borrowerId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = (DateTime.UtcNow + TimeSpan.FromDays(14)),
                Active = true };// Set the Active property to indicate the book is on loan

            // Update the book quantity
            book.Quantity--;

            //add loan to database
            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();

            LoanResponse response = new LoanResponse { Success = true, Message = "Book borrowed successfully.", Loan = loan };
            return response;
        }

        public async Task<LoanResponse> GetLoanByIdAsync(int? loanId)
        {
            // Check if loanId is not null
            if (loanId == null)
                throw new ArgumentNullException(nameof(loanId));

            // Get matching loan from database
            Loan? loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.ID == loanId);

            if (loan == null)
                return null;

            LoanResponse loanResponse = new LoanResponse() { Success = true , Message = "this is loan that you want !", Loan = loan};
            return loanResponse;
        }

        public async Task<int> GetActiveLoanDurationAsync(int loanId)
        {
            //Get loan by id
            Loan loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.ID == loanId);
            //check for null situation 
            if (loan == null || loan.ReturnDate == null) return 0;
            //calculate duration
            if (loan.Active)
            {
                TimeSpan duration = loan.ReturnDate.Value - loan.LoanDate;
                return (int)duration.TotalDays;
            }
            return -1;
        }
        public async Task<int> GetInactiveLoanDurationAsync(int loanId)
        {
            //Get loan by id
            Loan loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.ID == loanId);
            //check for null situation 
            if (loan == null || loan.ReturnDate == null) return 0;
            //calculate duration
            if (!loan.Active)
            {
                TimeSpan duration = loan.ReturnDate.Value - loan.LoanDate;
                return (int)duration.TotalDays;
            }
            return -1;
        }

        public async Task<IEnumerable<LoanResponse>> GetLoansByBookAsync(int bookId)
        {
            //check if book exist
            Book? book = await _dbContext.Books.FirstOrDefaultAsync(b => b.ID == bookId);
            if(book == null ) return Enumerable.Empty<LoanResponse>();

            //finding loans by bookId
            List<Loan> loans = await _dbContext.Loans.Where(l => l.BookID == bookId).ToListAsync();
            List<LoanResponse> loanResponses = loans.
                Select(l => new LoanResponse() { Message = "loans finded!", Success = true, Loan = l }).ToList();

            return loanResponses;

        }

        public async Task<IEnumerable<LoanResponse>> GetLoansByBorrowerAsync(int borrowerId)
        {
            // Check if the borrower exists
            Borrower borrower = await _dbContext.Borrowers.FirstOrDefaultAsync(b => b.ID == borrowerId);
            if (borrower == null)
            {
                return Enumerable.Empty<LoanResponse>(); // Return an empty collection if the borrower is not found
            }

            // Find loans by borrowerId
            List<Loan> loans = await _dbContext.Loans.Where(l => l.BorrowerID == borrowerId).ToListAsync();

            // Map Loan objects to LoanResponse objects
            List<LoanResponse> loanResponses = loans.Select(l => new LoanResponse
            {
                Success = true,
                Message = "Loans found!",
                Loan = l
            }).ToList();

            return loanResponses;
        }

        public async Task<IEnumerable<LoanResponse>> GetOverdueLoansAsync()
        {
            // Get the current date in UTC
            DateTime currentDateUtc = DateTime.UtcNow;

            // Find the overdue loans
            List<Loan> overdueLoans = await _dbContext.Loans
                .Where(l => l.ReturnDate < currentDateUtc && l.Active)
                .ToListAsync();

            // Map Loan objects to LoanResponse objects
            List<LoanResponse> loanResponses = overdueLoans.Select(l => new LoanResponse
            {
                Success = true,
                Message = "Overdue loans found!",
                Loan = l
            }).ToList();

            return loanResponses;
        }

        public async Task<int> GetTotalLoansCountAsync()
        {
            int loanCount = await _dbContext.Loans.CountAsync();
            return loanCount;
        }

        public async Task<LoanResponse> ReturnBookAsync(int loanId)
        {
            // Check if the loan exists
            Loan? loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.ID == loanId);
            if (loan == null)
            {
                return null;
            }

            // Set the Active property to indicate the book is returned
            loan.Active = false;
            loan.ReturnDate = DateTime.UtcNow;
            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();

            LoanResponse loanResponse = new LoanResponse { Success = true, Message = "Book returned successfully.", Loan = loan };
            return loanResponse;
        }

        public async Task<LoanResponse> ExtendLoanAsync(int loanId, int extensionDays)
        {
            // Check if the loan exists
            Loan loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.ID == loanId);
            if (loan == null)
            {
                return null;
            }

            // Check if the loan is active
            if (!loan.Active)
            {
                return new LoanResponse { Success = false, Message = "Cannot extend an inactive loan." };
            }

            // Calculate the new return date
            DateTime newReturnDate = loan.ReturnDate.Value.AddDays(extensionDays);

            // Update the return date of the loan
            loan.ReturnDate = newReturnDate;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();

            LoanResponse loanResponse = new LoanResponse { Success = true, Message = "Loan extended successfully.", Loan = loan };
            return loanResponse;
        }

        public async Task<IEnumerable<LoanResponse>> GetActiveLoansAsync()
        {
            // Get the active loans
            List<Loan> activeLoans = await _dbContext.Loans
                .Where(l => l.Active)
                .ToListAsync();

            // Map Loan objects to LoanResponse objects
            List<LoanResponse> loanResponses = activeLoans.Select(l => new LoanResponse
            {
                Success = true,
                Message = "Active loans found!",
                Loan = l
            }).ToList();

            return loanResponses;
        }

    }
}
