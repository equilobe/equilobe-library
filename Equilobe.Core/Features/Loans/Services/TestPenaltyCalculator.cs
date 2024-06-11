using Equilobe.Core.Features.Loans.Interfaces;
using Equilobe.Core.Shared.Models;

namespace Equilobe.Core.Features.Loans.Services
{
    public class TestPenaltyCalculator : IPenaltyCalculator
    {
        public decimal CalculatePenalty(Money rentPrice, DateTime dueDate, DateTime returnDate, int differenceQualityState)
        {
            var overdueDays = (returnDate - dueDate).Days;
            return overdueDays > 0 ? overdueDays * (rentPrice.Amount * 0.01m) : 0;
        }
    }
}
