using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.Features.Books.Queries;

public class GetAvailableBooksQuery : IRequest<int>
{
    public string ISBN { get; }

    public GetAvailableBooksQuery(string isbn)
    {
        ISBN = isbn;
    }
}

public class GetAvailableBooksQueryHandler : IRequestHandler<GetAvailableBooksQuery, int>
{
    private readonly ILibraryDbContext dbContext;

    public GetAvailableBooksQueryHandler(ILibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> Handle(GetAvailableBooksQuery request, CancellationToken cancellationToken)
    {
        var books = this.dbContext.Books.Where(b => b.Metadata.ISBN == request.ISBN);
        if (!books.Any()) throw new KeyNotFoundException(nameof(BookMetadata));

        return await books.CountAsync(b => b.IsAvailable, cancellationToken);
    }
}
