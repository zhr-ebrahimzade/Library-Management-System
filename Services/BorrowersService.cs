using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BorrowersService : IBorrowersService
    {
        private readonly LibraryDbContext _dbContext;
        public BorrowersService(LibraryDbContext libraryDbContext)
        {
                _dbContext= libraryDbContext;
        }
        public async Task<BorrowerResponse> AddBorrowerAsync(BorrowerAddRequest borrowerRequest)
        {
            if(borrowerRequest== null) throw new ArgumentNullException(nameof(borrowerRequest));

            ValidationHelper.Validate(borrowerRequest);

            Borrower newBorrower = borrowerRequest.ToBorrower();

            await _dbContext.Borrowers.AddAsync(newBorrower);
            await _dbContext.SaveChangesAsync();

            BorrowerResponse  borrowerResponse = newBorrower.ToBorrowerResponse();
            return borrowerResponse;
        }

        public async Task<IEnumerable<BorrowerResponse>> BorrowerSearchByAsync(string field, string searchValue)
        {
            var query = _dbContext.Borrowers.AsQueryable();

            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(searchValue))
            {
                switch (field)
                {
                    case "Name":
                        query = query.Where(b => (b.FirstName + " " + b.LastName).
                        ToLower().Contains(searchValue.ToLower()));
                        break;

                    case nameof(Borrower.LastName):
                        query = query.Where(b => b.LastName.ToLower().Contains(searchValue.ToLower()));
                        break;

                    case nameof(Borrower.Email):
                        query = query.Where(b => b.Email.ToLower().Contains(searchValue.ToLower()));
                        break;

                    case nameof(Borrower.PhoneNumber):
                        query = query.Where(b => b.PhoneNumber.ToLower().Contains(searchValue.ToLower()));
                        break;

                    default:
                        // Invalid field, return all borrowers
                        break;
                }
            }

            var result = await query.ToListAsync();
            List<BorrowerResponse> borrowerResponses = result.Select(b => b.ToBorrowerResponse()).ToList();

            return borrowerResponses;
        }

        public async Task<IEnumerable<BorrowerResponse>> GetAllBorrowersAsync()
        {
            IEnumerable<Borrower> borrowers = await _dbContext.Borrowers.ToListAsync();
            return borrowers.Select(b => b.ToBorrowerResponse());
        }
        public async Task<BorrowerResponse?> GetBorrowerByIdAsync(int? borrowerId)
        {
            // Check if borrowerId is not null
            if (borrowerId == null)
                return null;
            // Get matching borrower from database
            Borrower? borrower = await _dbContext.Borrowers.FirstOrDefaultAsync(b => b.ID == borrowerId);

            if (borrower == null)
                return null;

            BorrowerResponse borrowerResponse = borrower.ToBorrowerResponse();
            return borrowerResponse;
        }
        public async Task<bool> DeleteBorrowerAsync(int? borrowerId)
        {
            // Check if borrowerId is not null
            if (borrowerId == null)
                throw new ArgumentNullException(nameof(borrowerId));

            // Get the borrower from the database
            Borrower? borrower = await _dbContext.Borrowers.FirstOrDefaultAsync(b => b.ID == borrowerId);

            if (borrower == null)
                return false;

            // Remove the borrower from the database
            _dbContext.Borrowers.Remove(borrower);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(int borrowerId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetBorrowedBooksCountAsync(int borrowerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LoanResponse>> GetBorrowerLoanHistoryAsync(int borrowerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BorrowerResponse>> GetOverdueBorrowersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BorrowerResponse>> SearchBorrowersAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

     
    }
}
