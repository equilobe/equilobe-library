using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Equilobe.Core.Features.Loans.Commands;

public class ReturnBookCommand : IRequest
{
    public required Guid BookId { get; init; }
    public DateTime? ReturnDate { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required BookQualityState BookQuality { get; init; }
}

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
{
    private readonly ILibraryDbContext dbContext;

    public ReturnBookCommandHandler(ILibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var loan = await this.dbContext.Loans
            .FirstOrDefaultAsync(l => l.BookId == request.BookId, cancellationToken)
            ?? throw new KeyNotFoundException(nameof(Loan));

        loan.ReturnBook(request.BookQuality, request.ReturnDate ?? DateTime.UtcNow);

        await this.dbContext.SaveChangesAsync(cancellationToken);
    }
}
