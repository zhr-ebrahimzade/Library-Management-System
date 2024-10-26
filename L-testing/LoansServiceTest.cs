using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTests
{
    public class LoansServiceTest
    {
        private readonly ILoansService _loanService;
        public LoansServiceTest()
        {
            List<Loan> initialLoanList = new List<Loan>();

            DbContextMock<LibraryDbContext> dbContextMock = new DbContextMock<LibraryDbContext>(
                new DbContextOptionsBuilder<LibraryDbContext>().Options);
            LibraryDbContext libraryDb = dbContextMock.Object;

            dbContextMock.CreateDbSetMock<Loan>(t => t.Loans, initialLoanList);
            _loanService = new LoansService(libraryDb);
        }
    }
}
