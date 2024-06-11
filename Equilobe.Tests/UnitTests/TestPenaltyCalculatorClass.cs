using Equilobe.Core.Features.Loans.Services;
using Equilobe.Core.Shared.Models;

namespace Equilobe.Tests.UnitTests
{
    public class TestPenaltyCalculatorTests
    {
        [Fact]
        public void CalculatePenalty_ShouldReturnCorrectPenaltyWithoutQualityState()
        {
            // Arrange
            var penaltyCalculator = new TestPenaltyCalculator();
            var rentPrice = new Money(100, Currency.RON);
            var dueDate = new DateTime(2023, 1, 1);
            var returnDate = new DateTime(2023, 1, 5); // 4 days overdue

            // Act
            var penalty = penaltyCalculator.CalculatePenalty(rentPrice, dueDate, returnDate, 0);

            // Assert
            Assert.Equal(4 * (100 * 0.01m), penalty);
        }
    }
}
