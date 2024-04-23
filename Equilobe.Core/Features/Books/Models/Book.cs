using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared.SeedWork;

namespace Equilobe.Core.Features.Books;

public class Book : Entity
{
    public bool IsAvailable { get; private set; }
    public BookQualityState QualityState { get; private set; }
    public Money RentPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public BookMetadata Metadata { get; private set; }

    private Book() { }

    public Book(Money rentPrice, BookMetadata metadata)
    {
        QualityState = BookQualityState.New;
        CreatedAt = DateTime.UtcNow;
        IsAvailable = true;
        RentPrice = rentPrice;
        Metadata = metadata;
    }

    public void LoanBook()
    {
        IsAvailable = false;
    }

    public void ReturnBook(BookQualityState qualityState)
    {
        QualityState = qualityState;
        IsAvailable = true;
    }

    public void SetNewRentPrice(Money newRentPrice)
    {
        RentPrice = newRentPrice;
    }
}
