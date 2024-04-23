using Equilobe.Core.Shared.SeedWork;

namespace Equilobe.Core.Features.Books;

public class Author : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? MiddleName { get; private set; }

    public Author(string firstName, string lastName, string? middleName = null) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName ?? null;
    }

    public override string ToString()
    {
        return MiddleName != null
            ? $"{FirstName} {MiddleName} {LastName}"
            : $"{FirstName} {LastName}";
    }
}
