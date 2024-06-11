using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Shared;
using Equilobe.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.Features.Books.Queries;

public class GetBooksQuery : IRequest<List<BookDTO>>
{
    public string? Title { get; }
    public string? QualityState { get; }
    public bool? IsAvailable { get; }
    public string SortBy { get; }
    public bool SortAscending { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetBooksQuery(string? title, string? qualityState, bool? isAvailable, string? sortBy, bool sortAscending, int pageNumber, int pageSize)
    {
        this.Title = title;
        this.QualityState = qualityState;
        this.IsAvailable = isAvailable;
        this.SortBy = !string.IsNullOrWhiteSpace(sortBy) ? sortBy : "id";
        this.SortAscending = sortAscending;
        this.PageNumber = pageNumber != default(int) ? pageNumber : 1;
        this.PageSize = pageSize != default(int) ? pageSize : 10;
    }

    public GetBooksQuery()
    {
        SortBy = "id";
        PageNumber = 1;
        PageSize = 10;
    }
}

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDTO>>
{
    private readonly ILibraryDbContext dbContext;

    public GetBooksQueryHandler(ILibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<BookDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await this.dbContext.Books
            .Include(b => b.Metadata.Author)
            .ToListAsync(cancellationToken);

        if(!string.IsNullOrWhiteSpace(request.Title))
        {
            books = books.Where(b => b.Metadata.Title.Equals(request.Title)).ToList();
        }

        BookQualityState qualityState;
        if (!string.IsNullOrWhiteSpace(request.QualityState) && Enum.TryParse(request.QualityState, out qualityState))
        {
            books = books.Where(b => b.QualityState.Equals(qualityState)).ToList();
        }

        if (request.IsAvailable.HasValue)
        {
            books = books.Where(b => b.IsAvailable == request.IsAvailable.Value).ToList();
        }

        switch (request.SortBy.ToLower())
        {
            case "title":
                books = request.SortAscending ? books.OrderBy(b => b.Metadata.Title).ToList() : books.OrderByDescending(b => b.Metadata.Title).ToList();
                break;
            case "creationdate":
                books = request.SortAscending ? books.OrderBy(b => b.CreatedAt).ToList() : books.OrderByDescending(b => b.CreatedAt).ToList();
                break;
            default:
                books = request.SortAscending ? books.OrderBy(b => b.Id).ToList() : books.OrderByDescending(b => b.Id).ToList();
                break;
        }

        var totalBooks = books.Count();
        // var totalPages = (int)Math.Ceiling(totalBooks / (double)request.PageSize);
        books = books.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

        return books.Select(book => new BookDTO(book)).ToList();
    }
}
