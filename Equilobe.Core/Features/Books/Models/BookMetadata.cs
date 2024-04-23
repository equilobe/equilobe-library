using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared.SeedWork;

namespace Equilobe.Core.Features.Books;

public class BookMetadata : ValueObject
{
    private BookMetadata() { }

    public BookMetadata(string title, Author author, string isbn) : base()
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
        Author = author ?? throw new ArgumentNullException(nameof(author));
    }

    public string Title { get; private set; }
    public string ISBN { get; private set; }
    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Author;
        yield return ISBN;
    }
}
