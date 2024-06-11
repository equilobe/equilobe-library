using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared.SeedWork;
using Equilobe.Core.DomainEvents;
using Equilobe.Core.Features.Loans.Services;
using Equilobe.Core.Features.Loans.Interfaces;

namespace Equilobe.Core.Features.Loans;

public class Loan : Entity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime LoanDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public Money PaidAmount { get; private set; }

    private Loan() { }

    public Loan(Guid bookId, Guid userId, DateTime? dueDate = null) : base()
    {
        BookId = bookId;
        UserId = userId;
        LoanDate = DateTime.UtcNow;
        DueDate = dueDate ?? LoanDate.AddDays(14);
        ReturnDate = null;
        PaidAmount = new Money(0, Currency.RON);
        AddDomainEvent(new BookLoanedEvent(bookId));
    }

    public void ReturnBook(BookQualityState qualityState, DateTime returnDate)
    {
        ReturnDate = returnDate;
        AddDomainEvent(new BookReturnedEvent(BookId, qualityState, returnDate));
    }

    public decimal CalculatePenalty(Money rentPrice, int differenceQualityState, IPenaltyCalculator penaltyCalculator)
    {
        var totalAmount = penaltyCalculator.CalculatePenalty(rentPrice, DueDate, ReturnDate.Value, differenceQualityState);
        PaidAmount = new Money(totalAmount, rentPrice.Currency);
        return totalAmount;
    }
}
