using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTests
{
    public class BorrowersServiceTest
    {
        private readonly IBorrowersService _borrowersService;
        private readonly IFixture _fixture;

        public BorrowersServiceTest()
        {
            List<Borrower> initialBorrower = new List<Borrower>();
            DbContextMock<LibraryDbContext> dbContextMock = new DbContextMock<LibraryDbContext>(
               new DbContextOptionsBuilder<LibraryDbContext>().Options);

            LibraryDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock<Borrower>(temp => temp.Borrowers, initialBorrower);
            _borrowersService = new BorrowersService(dbContext);
            _fixture  = new Fixture();
        }



        #region AddBorrowerAsync
        //When BorrowerAddRequest is null , it should throw ArgumentNullException
        [Fact]
        public async Task AddBorrowerrAsync_NullBorrowerRequest_ThrowsArgumentNullExeption()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                //Act
                await _borrowersService.AddBorrowerAsync(borrowerAddRequest);
            });
        }

        //When FirstName is null , it should throw ArgumentException

        [Fact]
        public async Task AddBorrowerAsync_NullOrEmptyFirstName_ThrowsArgumentException()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = new BorrowerAddRequest 
            { FirstName = null as string , LastName = "something", Address = "abc street",
                DateOfBirth = DateTime.UtcNow , Email = "someone@example.com" , PhoneNumber = "0123456789"};

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _borrowersService.AddBorrowerAsync(borrowerAddRequest);
            });
        }


        //When FirstName is null , it should throw ArgumentException

        [Fact]
        public async Task AddBorrowerAsync_NullOrEmptyEmail_ThrowsArgumentException()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = new BorrowerAddRequest
            {
                FirstName = "someone",
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = null as string,
                PhoneNumber = "0123456789"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _borrowersService.AddBorrowerAsync(borrowerAddRequest);
            });
        }



        //test that a Borrower successfully added to database
        [Fact]
        public async Task AddBorrowerAsync_ValidBorrowerRequest_SuccessfullyAdded()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = new BorrowerAddRequest
            {
                FirstName = "someone",
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = "someone@example.com",
                PhoneNumber = "0123456789"
            };
            //Act
            BorrowerResponse? response = await _borrowersService.AddBorrowerAsync(borrowerAddRequest);

            //Assert
            Assert.NotNull(response);
            //Assert.True(authorResponse.ID > 0);
            Assert.Equal(borrowerAddRequest.FirstName, response.FirstName);
            Assert.Equal(borrowerAddRequest.LastName, response.LastName);
            Assert.Equal(borrowerAddRequest.PhoneNumber, response.PhoneNumber);
        }


        #endregion



        #region GetAllBorrowersAsync
        //should return an empty list by defult
        [Fact]
        public async Task GetAllBorrowersAsync_ByDefult_EmptyList()
        {
            //Arrange 

            //Act
            IEnumerable<BorrowerResponse>? borrowerResponse_FromGet = await _borrowersService.GetAllBorrowersAsync();
            //Assert
            Assert.Empty(borrowerResponse_FromGet);
        }

        //add few Borrower , then call GetAllBorrowersAsync() and it should return all Borrower
        [Fact]
        public async Task GetAllBorrowersAsync_AddFewBorrowers_ShouldReturnAllBorrowers()
        {
            // Arrange
            int numberOfBorrowers = 3; // Number of borrowers to add

            // Add multiple borrowers with unique IDs
            BorrowerAddRequest? borrowerAddRequest = new BorrowerAddRequest
            {
                FirstName = "someone",
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = "someone@example.com",
                PhoneNumber = "0123456789"
            };
            BorrowerResponse? borrowerResponse = await _borrowersService.AddBorrowerAsync(borrowerAddRequest);
            BorrowerAddRequest? borrowerAddRequest1 = new BorrowerAddRequest
            {
                FirstName = "someone",
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = "someone@example.com",
                PhoneNumber = "0123456789"
            };
            BorrowerResponse? borrowerResponse1 = await _borrowersService.AddBorrowerAsync(borrowerAddRequest); 
            BorrowerAddRequest? borrowerAddRequest2 = new BorrowerAddRequest
            {
                FirstName = null as string,
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = "someone@example.com",
                PhoneNumber = "0123456789"
            };
            BorrowerResponse? borrowerResponse2 = await _borrowersService.AddBorrowerAsync(borrowerAddRequest);

            IEnumerable<BorrowerResponse>? expectedBorrowers = new List<BorrowerResponse>() { borrowerResponse, borrowerResponse1, borrowerResponse2 };

            // Act
            IEnumerable<BorrowerResponse>? actualBorowers = await _borrowersService.GetAllBorrowersAsync();

            // Assert
            Assert.NotNull(actualBorowers);
            Assert.Contains(borrowerResponse1, actualBorowers);
        }


        #endregion



        #region GetAuthorByIdAsync
        //if borrowerId is null , it should return null as borrowerResponse
        [Fact]
        public async Task GetBorrowerByIdAsync_NullBorrowerId_NullBorrowerResponse()
        {
            //Arrange 
            int? borrowerId = null;

            //Act
            BorrowerResponse? response = await _borrowersService.GetBorrowerByIdAsync(borrowerId);

            //Assert
            Assert.Null(response);
        }


        //if supplay valid borrowerId should return valid Borrower details as BorrowerResponse object
        [Fact]
        public async Task GetBorrowerByIdAsync_ExistingBorrowerId_ReturnBorrowerResponse()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = new BorrowerAddRequest
            {
                FirstName = "someone",
                LastName = "something",
                Address = "abc street",
                DateOfBirth = DateTime.UtcNow,
                Email = "someone@example.com",
                PhoneNumber = "0123456789"
            };
            //todo: Mock AddBorrowerAsync
            BorrowerResponse borrowerResponse_FromAdd = await _borrowersService.AddBorrowerAsync(borrowerAddRequest);
            //Act
            BorrowerResponse? borrowerResponse_FromGet = await _borrowersService.GetBorrowerByIdAsync(borrowerResponse_FromAdd.ID);

            //Assert
            Assert.Equal(borrowerResponse_FromAdd.FirstName, borrowerResponse_FromGet.FirstName);
        }


        #endregion



        #region DeleteBorrowerAsync

        //This test case validates that the method throws an ArgumentNullException when a null borrowerID is provided
        [Fact]
        public async Task DeleteAuthorAsync_NullAuthorId_ThrowsArgumentNullException()
        {
            //Arrange 
            int? borrowerId = null;


            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act
            await _borrowersService.DeleteBorrowerAsync(borrowerId));
        }


        // if we supplay valid borrowerID it should return True
        [Fact]
        public async Task DeleteBorrowerAsync_validBorrowerId_ReturnTrue()
        {
            //Arrange 
            BorrowerAddRequest? borrowerAddRequest = _fixture.Build<BorrowerAddRequest>()
                .With(t => t.Email, "someone@example.com")
                .With(t => t.PhoneNumber , "0123456789")
                .Create();

            //todo: Mock AddBorrowerAsync
            BorrowerResponse borrowerResponse = await _borrowersService.AddBorrowerAsync(borrowerAddRequest);

            //Act
            bool deleted = await _borrowersService.DeleteBorrowerAsync(borrowerResponse.ID);
            //Assert
            Assert.True(deleted);
        }


        // if we supplay invalid borrowerID it should return True
        [Fact]
        public async Task DeleteBorrowerAsync_InvalidBorrowerId_ReturnFalse()
        {

            //Act
            bool deleted = await _borrowersService.DeleteBorrowerAsync(999);
            //Assert
            Assert.False(deleted);
        }

        #endregion
    }
}
