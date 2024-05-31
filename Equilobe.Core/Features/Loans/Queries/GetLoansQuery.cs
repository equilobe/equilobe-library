using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Features.Books.Queries;
using Equilobe.Core.Features.Loans.DTO;
using Equilobe.Core.Shared;
using Equilobe.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equilobe.Core.Features.Loans.Queries
{
    public class GetLoansQuery : IRequest<List<LoanDTO>>
    {
    }

    public class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, List<LoanDTO>>
    {
        private readonly ILibraryDbContext _dbContext;

        public GetLoansQueryHandler(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LoanDTO>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _dbContext.Loans
                .ToListAsync(cancellationToken);
            return loans.Select(book => new LoanDTO(book, string.Empty)).ToList();
        }

        public async Task<List<LoanDTO>> Handle(GetLoansQuery request, CancellationToken cancellationToken, string? bookTitle,
            DateTime? dueDateStart, DateTime? dueDateEnd, SortingState sortId, int pageSize = 10)
        {
            if (pageSize > 50)
            {
                pageSize = 50;
            }
            if (bookTitle == null)
            {
                bookTitle = string.Empty;
            }
            
            var loans = await _dbContext.Loans
                .ToListAsync(cancellationToken);

            var books = await _dbContext.Books
                        .Where(b => b.Metadata.Title.Contains(bookTitle))
                        .Select(b => new { b.Id, b.Metadata.Title })
                        .ToListAsync(cancellationToken);

            var bookIds = books.Select(b => b.Id).ToList();

            loans = loans.Where(l => bookIds.Contains(l.BookId)).ToList();
            
            if (dueDateStart != null && dueDateEnd != null)
            {
                loans = loans.Where(l => l.DueDate > dueDateStart.Value && l.DueDate < dueDateEnd.Value).ToList();
            }

            List<LoanDTO> loansWithTitles = loans.Join(books, 
                loan => loan.BookId, 
                book => book.Id, 
                (loan, book) => new LoanDTO(loan, book.Title)).ToList();

            if (sortId == SortingState.Ascending)
            {
                loansWithTitles = loansWithTitles.OrderBy(x => x.Id).ToList();
            }

            if (sortId == SortingState.Descending)
            {
                loansWithTitles = loansWithTitles.OrderByDescending(x => x.Id).ToList();
            }

            loansWithTitles = loansWithTitles.Take(pageSize).ToList();

            return loansWithTitles.Select(a => a).ToList();
        }





    }
}
