using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Features.Loans.DTO;
using Equilobe.Core.Features.Loans.Queries;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.Features.Loans.Handlers
{
    public  class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, List<LoanDTO>>
    {
        private readonly ILibraryDbContext dbContext;

        public GetLoansQueryHandler(ILibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<LoanDTO>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await dbContext.Loans
            .Include(l => l.BookId)
            .ToListAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.BookTitle))
            {
                var books = await this.dbContext.Books
                    .Include(b => b.Metadata.Title)
                    .ToListAsync(cancellationToken);

                var book = books.FirstOrDefault();

                if(book != null)
                {
                    loans = loans.Where(l => l.BookId.Equals(book.Id)).ToList();
                }
            }

            if (request.BookId.HasValue && request.BookId != Guid.Empty)
            {
                loans = loans.Where(l => l.BookId.Equals(request.BookId)).ToList();
            }

            if(request.UserId.HasValue && request.UserId != Guid.Empty)
            {
                loans = loans.Where(l => l.UserId.Equals(request.BookId)).ToList();
            }

            switch (request.SortBy.ToLower())
            {
                case "bookId":
                    loans = request.SortAscending ? loans.OrderBy(l => l.BookId).ToList() : loans.OrderByDescending(l => l.BookId).ToList();
                    break;
                default:
                    loans = request.SortAscending ? loans.OrderBy(l => l.UserId).ToList() : loans.OrderByDescending(l => l.UserId).ToList();
                    break;
            }

            var totalBooks = loans.Count();
            loans = loans.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();   


            return loans.Select(loan => new LoanDTO(loan)).ToList();
        }
    }
}
