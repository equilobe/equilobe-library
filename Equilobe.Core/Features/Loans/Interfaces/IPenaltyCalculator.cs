using Equilobe.Core.Shared.Models;

namespace Equilobe.Core.Features.Loans.Interfaces
{
    public interface IPenaltyCalculator
    {
        decimal CalculatePenalty(Money rentPrice, DateTime dueDate, DateTime returnDate, int differenceQualityState);
    }
}
