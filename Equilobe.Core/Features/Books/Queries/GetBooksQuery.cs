using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
}
