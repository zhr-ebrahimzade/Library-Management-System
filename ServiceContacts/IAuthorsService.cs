using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IAuthorsService
    {
        Task<AuthorResponse> AddAuthorAsync(AuthorAddRequest authorRequest);

        Task<AuthorResponse?> GetAuthorByIdAsync(int? authorId);

        Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync();

        Task<IEnumerable<AuthorResponse>> SearchAuthorsAsync(string? searchTerm);

        //Task<AuthorResponseDTO> UpdateAuthorAsync(int authorId, AuthorUpdateRequestDTO authorRequest);

        Task<bool> DeleteAuthorAsync(int? authorId);

        Task<IEnumerable<BookResponse>> GetBooksByAuthorAsync(int? authorId);

        Task<AuthorResponse?> GetAuthorsOfBookAsync(int? bookId);

        Task<IEnumerable<AuthorResponse>> GetPopularAuthorsAsync(int count);
    }
}
