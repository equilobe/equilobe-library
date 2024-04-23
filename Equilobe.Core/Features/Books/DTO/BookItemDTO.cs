namespace Equilobe.Core.Features.Books.DTO;

public class BookItemDTO
{
    public BookItemDTO(Book bookItem)
    {
        ID = bookItem.Id.ToString();
        IsAvailable = bookItem.IsAvailable.ToString();
        QualityState = Enum.GetName(bookItem.QualityState) ?? throw new ArgumentNullException(Enum.GetName(bookItem.QualityState));
        RentPrice = bookItem.RentPrice.ToString() ?? throw new ArgumentNullException(nameof(bookItem.RentPrice));
        DateRegistered = bookItem.CreatedAt.ToLongDateString();
    }

    public string ID { get; }
    public string IsAvailable { get; }
    public string QualityState { get; }
    public string RentPrice { get; }
    public string DateRegistered { get; }
}
