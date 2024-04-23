using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Loans;
using Equilobe.Core.Features.Users;
using Equilobe.Core.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Equilobe.Infrastructure.Data;

public class LibraryDbContext : DbContext, ILibraryDbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Loan> Loans => Set<Loan>();

    public DbSet<Author> Authors => Set<Author>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
