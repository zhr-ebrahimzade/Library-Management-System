using Entities;
using ServiceContracts;
using Services;
using System.Diagnostics.Metrics;
using Moq;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Xunit;
using ServiceContracts.DTO;
using AutoFixture;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryTests
{
    public class BooksServiceTest
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorsService _authorsService;
        private readonly LibraryDbContext _dbContext;
        private readonly DbContextMock<LibraryDbContext> _dbContextMock;


        private readonly IFixture _fixture;
        public BooksServiceTest()
        {
            var booksInitialData = new List<Book>();
            var authorInitialData = new List<Author>();

            _dbContextMock = new DbContextMock<LibraryDbContext>
                (new DbContextOptionsBuilder<LibraryDbContext>().Options);

            _dbContext = _dbContextMock.Object;

            _dbContextMock.CreateDbSetMock(t => t.Books, booksInitialData);
            _dbContextMock.CreateDbSetMock(t => t.Authors, authorInitialData);

            _booksService = new BooksService(_dbContext);
            _authorsService = new AuthorsService(_dbContext);

            _fixture = new Fixture();
        }

        #region AddBookAsync
        //When BookAddRequest is null , it should throw ArgumentNullException
        [Fact]
        public async Task AddBookAsync_NullBookRequest_ThrowsArgumentNullException()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                //Act
                await _booksService.AddBookAsync(bookAddRequest);
            });
        }

        //When ISBN is null , it should throw ArgumentException

        [Fact]
        public async Task AddBookAsync_NullISBNBook_ThrowsArgumentException()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, null as string)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _booksService.AddBookAsync(bookAddRequest);
            });
        }




        //When Title is null , it should throw ArgumentException

        [Fact]
        public async Task AddBookAsync_NullTitleBook_ThrowsArgumentException()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
                .With(t => t.Title, null as string)
                .With(t => t.ISBN, "0590764845")
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _booksService.AddBookAsync(bookAddRequest);
            });
        }

        //When ISBN is duplicated , it should throw ArgumentException

        [Fact]
        public async Task AddBookAsync_DuplicateISBN_ThrowsArgumentException()
        {
            //Arrange 
            string duplicateISBN = "1234567890";
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, duplicateISBN)
                .Create(); BookAddRequest? bookAddRequest1 = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, duplicateISBN)
                .Create();


            await _booksService.AddBookAsync(bookAddRequest);
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _booksService.AddBookAsync(bookAddRequest1);
            });
        }

        //test that a book sccessfully added to database
        [Fact]
        public async Task AddBookAsync_ValidBookRequest_SuccessfullyAdded()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, "0590764845")
                .Create();

            //Act
            BookResponse? bookResponse = await _booksService.AddBookAsync(bookAddRequest);

            //Assert
            Assert.NotNull(bookResponse);
            //Assert.True(bookResponse.ID != null);
            Assert.Equal(bookAddRequest.ISBN, bookResponse.ISBN);
            Assert.Equal(bookAddRequest.Title, bookResponse.Title);
        }


        #endregion

         

        #region GetAllBooksAsync
        //should return an empty list by defult
        [Fact]
        public async Task GetAllBooksAsync_ByDefult_EmptyList()
        {
            //Arrange 

            //Act
            IEnumerable<BookResponse>? bookResponse_FromGet = await _booksService.GetAllBooksAsync();
            //Assert
            Assert.Empty(bookResponse_FromGet);
        }

        //Add few Book , then call GetAllBooks() , it should return all books
        [Fact]
        public async Task GetAllBooksAsync_AddSomeBooks_ShouldReturnAllBooks()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, "0590764845")
                .Create();
            BookAddRequest? bookAddRequest1 = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, "0591764845")
                .Create();
            BookAddRequest? bookAddRequest2 = _fixture.Build<BookAddRequest>()
                .With(t => t.ISBN, "0592764845")
                .Create();

            //todo: Mock AddBookAsync

            BookResponse bookResponse =  await _booksService.AddBookAsync(bookAddRequest);
            BookResponse bookResponse1 = await _booksService.AddBookAsync(bookAddRequest1);
            BookResponse bookResponse2 = await _booksService.AddBookAsync(bookAddRequest2);

            List<BookResponse> bookResponses_FromAdd = new List<BookResponse> { bookResponse, bookResponse1, bookResponse2 };

            // Act
            IEnumerable<BookResponse> bookResponses = await _booksService.GetAllBooksAsync();
            List<BookResponse> bookResponsesList = bookResponses.ToList();

            //Assert
            Assert.Equal(3, bookResponsesList.Count);

            Assert.Contains(bookAddRequest1.ISBN, bookResponsesList.Select(b => b.ISBN));
            Assert.Contains(bookAddRequest1.Title, bookResponsesList.Select(b => b.Title));

            Assert.Contains(bookAddRequest2.ISBN, bookResponsesList.Select(b => b.ISBN));
            Assert.Contains(bookAddRequest2.Title, bookResponsesList.Select(b => b.Title));

            Assert.Contains(bookAddRequest.ISBN, bookResponsesList.Select(b => b.ISBN));
            Assert.Contains(bookAddRequest.Title, bookResponsesList.Select(b => b.Title));
        }

        #endregion



        #region GetBookByIdAsync
        //if BookId is null , it should return null as bookResponse
        [Fact]
        public async Task GetBookByIdAsync_NullBookId_NullBookResponse()
        {
            //Arrange 
            int? BookId = null;

            //Act
            BookResponse response = await _booksService.GetBookByIdAsync(BookId);

            //Assert
            Assert.Null(response);
        }


        //if supplay valid bookId should return valid book details as bookResponse object
        [Fact]
        public async Task GetBookByIdAsync_ExistingBookId_ReturnBookResponse()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
            .With(t => t.ISBN, "0593764845")
            .Create();
            //todo: Mock AddBookAsync
            BookResponse bookResponse_FromAdd = await _booksService.AddBookAsync(bookAddRequest);
            //Act
            BookResponse bookResponse_FromGet = await _booksService.GetBookByIdAsync(bookResponse_FromAdd.ID);

            //Assert
            Assert.Equal(bookResponse_FromAdd.Title , bookResponse_FromGet.Title);
        }


        #endregion



        #region DeleteBookAsync

        //This test case validates that the method throws an ArgumentNullException when a null bookId is provided
        [Fact]
        public async Task DeleteBookAsync_NullBookId_ThrowsArgumentNullException()
        {
            //Arrange 
            int? bookId = null;

            
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async() =>  
            //Act
            await _booksService.DeleteBookAsync(bookId) );
        }


        // if we supplay valid bookId it should return True
        [Fact]
        public async Task DeleteBookAsync_validBookId_ReturnTrue()
        {
            //Arrange 
            BookAddRequest? bookAddRequest = _fixture.Build<BookAddRequest>()
            .With(t => t.ISBN, "0590764845")
            .Create();

            //todo: Mock AddBookAsync
            BookResponse bookResponse = await _booksService.AddBookAsync(bookAddRequest);

            //Act
            bool deleted = await _booksService.DeleteBookAsync(bookResponse.ID);
            //Assert
            Assert.True(deleted);
        }


        // if we supplay invalid bookId it should return True
        [Fact]
        public async Task DeleteBookAsync_InvalidBookId_ReturnFalse()
        {

            //Act
            bool deleted = await _booksService.DeleteBookAsync(999);
            //Assert
            Assert.False(deleted);
        }


        #endregion



        #region GetBooksByAuthorAsync

        //if authorId is null , return null
        [Fact]
        public async Task GetBooksByAuthorAsync_NullAuthorId_ReturnNull()
        {
            //Arrange 
            int? authorId = null;
            //Act
            IEnumerable<BookResponse>? bookResponse = await _booksService.GetBooksByAuthorAsync(authorId);

            //Assert
            Assert.Null(bookResponse);
        }


        //if authorId is not exist , return Empty List
        [Fact]
        public async Task GetBooksByAuthorAsync_AuthorIdNotExist_ReturnEmptyList()
        {
            //Arrange 
            int? authorId = 999;
            //Act
            IEnumerable<BookResponse>? bookResponse = await _booksService.GetBooksByAuthorAsync(authorId);

            //Assert
            // Assert
            Assert.NotNull(bookResponse);
            Assert.Empty(bookResponse);
        }

        //if author exist and supply valid id to method , it should return list of bookResponse
        [Fact]
        public async Task GetBooksByAuthorAsync_ValidAuthorId_ReturnsListOfBooks()
        {

            // Arrange
            //AuthorAddRequest authorAddRequest = _fixture.Create<AuthorAddRequest>();
            AuthorAddRequest? authorAddRequest = new AuthorAddRequest() { Name = "Zahra" };
            AuthorResponse authorResponse = await _authorsService.AddAuthorAsync(authorAddRequest);

            BookAddRequest bookAddRequest1 = _fixture.Build<BookAddRequest>()
                .With(t => t.AuthorID, authorResponse.ID)
                .With(t => t.ISBN, "0590764845")
                .Create();
            BookAddRequest bookAddRequest2 = _fixture.Build<BookAddRequest>()
                .With(t => t.AuthorID, authorResponse.ID)
                .With(t => t.ISBN, "0591764845")
                .Create();

            BookResponse bookResponse1 = await _booksService.AddBookAsync(bookAddRequest1);
            BookResponse bookResponse2 = await _booksService.AddBookAsync(bookAddRequest2);

            IEnumerable<BookResponse> expectedBookResponses = new List<BookResponse>() { bookResponse1, bookResponse2 };

            // Act
            IEnumerable<BookResponse>? actualBookResponses = await _booksService.GetBooksByAuthorAsync(authorResponse.ID);

            // Assert
            Assert.NotNull(actualBookResponses);
            Assert.Equal(expectedBookResponses.Count(), actualBookResponses.Count());
            //Assert.Equal(expectedBookResponses.Select(b => b.ID), actualBookResponses.Select(b => b.ID));
        }

        #endregion



    }
}