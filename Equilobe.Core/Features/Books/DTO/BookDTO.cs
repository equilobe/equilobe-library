using Equilobe.Core.Shared.Models;

namespace Equilobe.Core.Features.Books.DTO;

public class BookDTO
{
    public BookDTO(Book book)
    {
        IsAvailable = book.IsAvailable;
        QualityState = book.QualityState;
        RentPrice = book.RentPrice;
        CreatedAt = book.CreatedAt;
        Metadata = book.Metadata;
        Id = book.Id;
    }
    public Guid Id { get; private set; }
    public bool IsAvailable { get; private set; }
    public BookQualityState QualityState { get; private set; }
    public Money RentPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public BookMetadata Metadata { get; private set; }
}
