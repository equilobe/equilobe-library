using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Loans;
using Equilobe.Core.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.Shared;

public interface ILibraryDbContext
{
    DbSet<User> Users { get; }

    DbSet<Book> Books { get; }

    DbSet<Loan> Loans { get; }

    DbSet<Author> Authors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
