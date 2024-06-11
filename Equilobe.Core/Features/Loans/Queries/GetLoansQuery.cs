using Equilobe.Core.Features.Loans.DTO;
using MediatR;

namespace Equilobe.Core.Features.Loans.Queries
{
    public class GetLoansQuery : IRequest<List<LoanDTO>>
    {
        public string? BookTitle { get; }
        public Guid? BookId { get; }
        public Guid? UserId { get; }
        public string SortBy { get; }
        public bool SortAscending { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetLoansQuery(string? bookTitle, Guid? bookId, Guid? userId, string sortBy, bool sortAscending, int pageNumber, int pageSize) 
        {
            this.BookTitle = bookTitle;
            this.BookId = bookId;
            this.UserId = userId;
            this.SortBy = !string.IsNullOrWhiteSpace(sortBy) ? sortBy : "userId";
            this.SortAscending = sortAscending;
            this.PageNumber = pageNumber != default(int) ? pageNumber : 1;
            this.PageSize = pageSize != default(int) ? pageSize : 10;
        }

        public GetLoansQuery()
        {
            SortBy = "id";
            PageNumber = 1;
            PageSize = 10;
        }
    }
}