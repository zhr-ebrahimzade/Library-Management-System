using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class AuthorsServiceTest
    {
        private readonly IAuthorsService _authorsService;
        private readonly IFixture _fixture;
        private readonly DbContextMock<LibraryDbContext> _dbContextMock;
        private readonly LibraryDbContext _dbContext;
        public AuthorsServiceTest()
        {
            List<Author> initialAuthorsList = new List<Author>();
            _dbContextMock = new DbContextMock<LibraryDbContext>(
                new DbContextOptionsBuilder<LibraryDbContext>().Options ) ;
 
            _dbContext = _dbContextMock.Object;
            _dbContextMock.CreateDbSetMock<Author>(t => t.Authors, initialAuthorsList);

            _authorsService = new AuthorsService(_dbContext);
            _fixture = new Fixture();   
        }


        #region AddAuthorAsync
        //When AuthorAddRequest is null , it should throw ArgumentNullException
        [Fact]
        public async Task AddAuthorAsync_NullAuthorRequest_ThrowsArgumentNullExeption()
        {
            //Arrange 
            AuthorAddRequest? authorAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                //Act
                await _authorsService.AddAuthorAsync(authorAddRequest);
            });
        }

        //When Name is null , it should throw ArgumentException

        [Fact]
        public async Task AddAuthorAsync_NullOrEmptyName_ThrowsArgumentException()
        {
            //Arrange 
            AuthorAddRequest? authorAddRequest = new AuthorAddRequest { Name = null as string };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _authorsService.AddAuthorAsync(authorAddRequest);
            });
        }



        //test that a Author successfully added to database
        [Fact]
        public async Task AddAuthorAsync_ValidAuthorRequest_SuccessfullyAdded()
        {
            //Arrange 
            AuthorAddRequest? authorAddRequest = new AuthorAddRequest() { Name = "Zahra" };

            //Act
            AuthorResponse? authorResponse = await _authorsService.AddAuthorAsync(authorAddRequest);

            //Assert
            Assert.NotNull(authorResponse);
            //Assert.True(authorResponse.ID > 0);
            Assert.Equal(authorAddRequest.Name, authorResponse.Name);
        }


        #endregion




        #region GetAllAuthorsAsync
        //should return an empty list by defult
        [Fact]
        public async Task GetAllAuthorAsync_ByDefult_EmptyList()
        {
            //Arrange 

            //Act
            IEnumerable<AuthorResponse>? authorResponse_FromGet = await _authorsService.GetAllAuthorsAsync();
            //Assert
            Assert.Empty(authorResponse_FromGet);
        }

        //add few author , then call GetAllAuthorsAsync() and it should return all books
        [Fact]
        public async Task GetAllAuthorsAsync_AddFewAuthors_ShouldReturnAllAuhtors()
        {
            //// Arrange
            //int numberOfAuthors = 3; // Number of authors to add

            //// Add multiple authors with unique IDs
            //AuthorAddRequest authorAddRequest1 = new AuthorAddRequest { Name = "Author1" };
            //AuthorResponse authorResponse1 = await _authorsService.AddAuthorAsync(authorAddRequest1);

            //AuthorAddRequest authorAddRequest2 = new AuthorAddRequest { Name = "Author2" };
            //AuthorResponse authorResponse2 = await _authorsService.AddAuthorAsync(authorAddRequest2);

            //AuthorAddRequest authorAddRequest3 = new AuthorAddRequest { Name = "Author3" };
            //AuthorResponse authorResponse3 = await _authorsService.AddAuthorAsync(authorAddRequest3);

            //IEnumerable<AuthorResponse>? expectedAuthors = new List<AuthorResponse>() { authorResponse1, authorResponse2, authorResponse3 };

            //// Act
            //IEnumerable<AuthorResponse>? actualAuthors = await _authorsService.GetAllAuthorsAsync();

            //// Assert
            //Assert.NotNull(actualAuthors);
            //Assert.Contains(authorResponse1, actualAuthors);


            //Arrange 
            AuthorAddRequest authorAddRequest = _fixture.Build<AuthorAddRequest>()
                .Create(); 
            AuthorAddRequest authorAddRequest1 = _fixture.Build<AuthorAddRequest>()
                .Create(); 
            AuthorAddRequest authorAddRequest2 = _fixture.Build<AuthorAddRequest>()
                .Create();


            AuthorResponse? authorResponse = _dbContextMock.Object.Authors.Add(authorAddRequest.ToAuthor()).Entity.ToAuthorResponse();
            AuthorResponse? authorResponse1 = _dbContextMock.Object.Authors.Add(authorAddRequest.ToAuthor()).Entity.ToAuthorResponse();
            AuthorResponse? authorResponse2 = _dbContextMock.Object.Authors.Add(authorAddRequest.ToAuthor()).Entity.ToAuthorResponse();
            _dbContext.SaveChanges();

            //AuthorResponse authorResponse = await _authorsService.AddAuthorAsync(authorAddRequest);
            //AuthorResponse authorResponse1 = await _authorsService.AddAuthorAsync(authorAddRequest1);
            //AuthorResponse authorResponse2 = await _authorsService.AddAuthorAsync(authorAddRequest2);

            //IEnumerable<AuthorResponse>? expectedAuthors = new List<AuthorResponse>() { authorResponse, authorResponse1, authorResponse2 };
            List<AuthorResponse>? expectedAuthors = new List<AuthorResponse>();
            expectedAuthors.Add(authorResponse);

            //Act

            IEnumerable<AuthorResponse>? actualAuthors = await _authorsService.GetAllAuthorsAsync();

            //Assert
            Assert.Contains(authorResponse, actualAuthors);
          
        }


        #endregion



        #region GetAuthorByIdAsync
        //if authorId is null , it should return null as authorResponse
        [Fact]
        public async Task GetAuthorByIdAsync_NullAuthorId_NullAuthorResponse()
        {
            //Arrange 
            int? authorId = null;

            //Act
            AuthorResponse? response = await _authorsService.GetAuthorByIdAsync(authorId);

            //Assert
            Assert.Null(response);
        }


        //if supplay valid authorId should return valid Author details as AuthorResponse object
        [Fact]
        public async Task GetAuthorByIdAsync_ExistingaAuthorId_ReturnAuthorResponse()
        {
            //Arrange 
            AuthorAddRequest? authorAddRequest = _fixture.Build<AuthorAddRequest>()
            .Create();
            //todo: Mock AddAuthorAsync
            AuthorResponse authorResponse_FromAdd = await _authorsService.AddAuthorAsync(authorAddRequest);
            //Act
            AuthorResponse? authorResponse_FromGet = await _authorsService.GetAuthorByIdAsync(authorResponse_FromAdd.ID);

            //Assert
            Assert.Equal(authorResponse_FromAdd.Name, authorResponse_FromGet.Name);
        }


        #endregion



        #region DeleteAuthorAsync

        //This test case validates that the method throws an ArgumentNullException when a null authorId is provided
        [Fact]
        public async Task DeleteAuthorAsync_NullAuthorId_ThrowsArgumentNullException()
        {
            //Arrange 
            int? authorId = null;


            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act
            await _authorsService.DeleteAuthorAsync(authorId));
        }


        // if we supplay valid authorId it should return True
        [Fact]
        public async Task DeleteAuthorAsync_validAuthorId_ReturnTrue()
        {
            //Arrange 
            AuthorAddRequest? authorAddRequest = _fixture.Build<AuthorAddRequest>()
            .Create();

            //todo: Mock AddBookAsync
            AuthorResponse authorResponse = await _authorsService.AddAuthorAsync(authorAddRequest);

            //Act
            bool deleted = await _authorsService.DeleteAuthorAsync(authorResponse.ID);
            //Assert
            Assert.True(deleted);
        }


        // if we supplay invalid authorID it should return True
        [Fact]
        public async Task DeleteAuthorAsync_InvalidAuthorId_ReturnFalse()
        {

            //Act
            bool deleted = await _authorsService.DeleteAuthorAsync(999);
            //Assert
            Assert.False(deleted);
        }

        #endregion

    }
}
