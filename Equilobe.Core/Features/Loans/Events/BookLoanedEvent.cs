using Equilobe.Core.Features.Books;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.DomainEvents;

public class BookLoanedEvent : INotification
{
    public Guid bookId { get; }

    public BookLoanedEvent(Guid bookId)
    {
        this.bookId = bookId;
    }
}

public class BookLoanedEventHandler : INotificationHandler<BookLoanedEvent>
{
    private readonly ILibraryDbContext _dbContext;

    public BookLoanedEventHandler(ILibraryDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Handle(BookLoanedEvent notification, CancellationToken cancellationToken)
    {
        var book = await _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == notification.bookId, cancellationToken)
            ?? throw new KeyNotFoundException(nameof(BookMetadata));

        book.LoanBook();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
