using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Equilobe.Core.Features.Books.Commands;

public class AddBookCommand : IRequest
{
    public required string Title { get; init; }
    public required string AuthorFirstName { get; init; }
    public string? AuthorMiddleName { get; init; }
    public required string AuthorLastName { get; init; }
    public required string ISBN { get; init; }
    public required decimal RentPrice { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required Currency RentCurrency { get; init; }
}

public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
{
    private readonly ILibraryDbContext _dbContext;

    public AddBookCommandHandler(ILibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var rentPrice = new Money(request.RentPrice, request.RentCurrency);
        var author = await _dbContext.Authors
            .FirstOrDefaultAsync(a => a.FirstName == request.AuthorFirstName && a.LastName == request.AuthorLastName, cancellationToken)
            ?? new Author(request.AuthorFirstName, request.AuthorLastName, request.AuthorMiddleName);
        var bookMetadata = new BookMetadata(request.Title, author, request.ISBN);
        var book = new Book(rentPrice, bookMetadata);

        await _dbContext.Books.AddAsync(book, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
