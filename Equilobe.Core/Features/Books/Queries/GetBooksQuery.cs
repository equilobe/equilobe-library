using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Shared;
using Equilobe.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Equilobe.Core.Features.Books.Queries;

public class GetBooksQuery : IRequest<List<BookDTO>>
{
}

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDTO>>
{
    private readonly ILibraryDbContext _dbContext;

    public GetBooksQueryHandler(ILibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _dbContext.Books
            .Include(b => b.Metadata.Author)
            .ToListAsync(cancellationToken);
        return books.Select(book => new BookDTO(book)).ToList();
    }

    
    public async Task<List<BookDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken,
        string? bookTitle, BookQualityState? qualityState, bool? isAvailable, SortingState sortId, SortingState sortDate, SortingState sortTitle, int pageSize = 10)
    {
        if (pageSize > 50)
        {
            pageSize = 50;
        }
        if (bookTitle == null)
        {
            bookTitle = string.Empty;
        }
        var newGuid = new Guid();

        var books = await _dbContext.Books
            .Include(b => b.Metadata.Author)
            .Where(b => b.Metadata.Title.ToLower(CultureInfo.InvariantCulture).Contains(bookTitle.ToLower(CultureInfo.InvariantCulture)))
            .ToListAsync(cancellationToken);

        if (qualityState != null)
        {
            books = books.Where(b => b.QualityState == qualityState.Value).ToList();
        }
        if (isAvailable != null)
        {
            books = books.Where(b => b.IsAvailable == isAvailable.Value).ToList();
        }

        //cant find a reverse guid function, will treat guid with conditional
        if (sortId == SortingState.Descending)
        {
            books = books.OrderByDescending(b => b.Id)
            .ThenBy(b => (sortTitle == SortingState.None) ? string.Empty : (sortTitle == SortingState.Ascending) ? b.Metadata.Title : new string(b.Metadata.Title.Reverse().ToArray()))
            .ThenBy(b => (sortDate == SortingState.None) ? DateTime.Now : (sortDate == SortingState.Ascending) ? b.CreatedAt : DateTime.Now + (DateTime.MaxValue.AddYears(-4000) - b.CreatedAt)).ToList();
        } else
        {
            books = books.OrderBy(b => (sortId == SortingState.None) ? newGuid : b.Id)
            .ThenBy(b => (sortTitle == SortingState.None) ? string.Empty : (sortTitle == SortingState.Ascending) ? b.Metadata.Title : new string(b.Metadata.Title.Reverse().ToArray()))
            .ThenBy(b => (sortDate == SortingState.None) ? DateTime.Now : (sortDate == SortingState.Ascending) ? b.CreatedAt : DateTime.Now + (DateTime.MaxValue.AddYears(-4000) - b.CreatedAt)).ToList();
        }

        books= books.Take(pageSize).ToList();

        
        return books.Select(book => new BookDTO(book)).ToList();
    }


}
