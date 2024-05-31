namespace Equilobe.Core.Features.Loans.DTO;

public class LoanDTO
{
    public LoanDTO(Loan loan, string? bookTitle)
    {
        Id = loan.Id;
        LoanDate = loan.LoanDate;
        DueDate = loan.DueDate;
        ReturnDate = loan.ReturnDate;
        BookTitle = bookTitle;
    }
    public Guid Id { get; }
    public DateTime LoanDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; }
    public string? BookTitle { get; }
}
