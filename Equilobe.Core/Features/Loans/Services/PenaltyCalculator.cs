using Equilobe.Core.Features.Loans.Interfaces;
using Equilobe.Core.Shared.Models;

namespace Equilobe.Core.Features.Loans.Services
{
    public class PenaltyCalculator : IPenaltyCalculator
    {
        public decimal CalculatePenalty(Money rentPrice, DateTime dueDate, DateTime returnDate, int differenceQualityState)
        {
            decimal totalAmount = 0;
            if (differenceQualityState > 2)
            {
                totalAmount = differenceQualityState * rentPrice.Amount * 0.2m;
            }

            var overdueDays = (returnDate - dueDate).Days;
            if (overdueDays > 0)
            {
                totalAmount += overdueDays * (rentPrice.Amount * 0.01m);
            }

            return totalAmount;
        }
    }
}
