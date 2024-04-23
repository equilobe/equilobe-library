using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Loans;
using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.DomainEvents;

public class BookReturnedEvent : INotification
{
    public Guid BookId { get; }
    public DateTime ReturnDate { get; }
    public BookQualityState QualityState { get; }
    public BookReturnedEvent(Guid bookId, BookQualityState qualityState, DateTime returnDate)
    {
        BookId = bookId;
        QualityState = qualityState;
        ReturnDate = returnDate;
    }
}

public class BookReturnedEventHandler : INotificationHandler<BookReturnedEvent>
{
    private readonly ILibraryDbContext _dbContext;

    public BookReturnedEventHandler(ILibraryDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Handle(BookReturnedEvent notification, CancellationToken cancellationToken)
    {
        var bookTask = _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == notification.BookId, cancellationToken);
        var loanTask = _dbContext.Loans
            .FirstOrDefaultAsync(b => b.BookId == notification.BookId, cancellationToken);        
        var book = await bookTask ?? throw new KeyNotFoundException(nameof(BookMetadata));
        var loan = await loanTask ?? throw new KeyNotFoundException(nameof(Loan));

        var differenceQualityState = notification.QualityState - book.QualityState;

        loan.CalculatePenalty(book.RentPrice, differenceQualityState);

        book.ReturnBook(notification.QualityState);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
