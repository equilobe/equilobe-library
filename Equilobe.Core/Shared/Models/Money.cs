using Equilobe.Core.Shared.SeedWork;

namespace Equilobe.Core.Shared.Models;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    public Money(decimal amount, Currency currency)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<string> GetEqualityComponents()
    {
        yield return ToString();
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}
