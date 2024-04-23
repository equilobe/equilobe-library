using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Users;
using Equilobe.Core.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Equilobe.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<LibraryDbContextInitializer>();

        await initialiser.InitialiseAsync();

        await initialiser.TrySeedAsync();
    }
}

internal class LibraryDbContextInitializer
{
    private readonly ILogger<LibraryDbContextInitializer> _logger;
    private readonly LibraryDbContext _context;

    public LibraryDbContextInitializer(LibraryDbContext context, ILogger<LibraryDbContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            await SeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedAsync()
    {
        if (!_context.Users.Any())
        {
            _context.Users.Add(new User("Bibliotecar", UserRole.Librarian));
            _context.Users.Add(new User("Cititor1", UserRole.Reader));
            _context.Users.Add(new User("Cititor2", UserRole.Reader));
        }

        if (!_context.Books.Any())
        {
            var bookMetadata1 = new BookMetadata("Atomic Habits", new Author("James", "Clear"), "9780735211292");
            _context.Books.Add(new Book(new Money(70m, Currency.RON), bookMetadata1));

            var bookMetadata2 = new BookMetadata("The Pragmatic Programmer", new Author("Andrew", "Hunt"), "9780201616224");
            _context.Books.Add(new Book(new Money(50m, Currency.RON), bookMetadata2));

            var bookMetadata3 = new BookMetadata("Clean Code", new Author("Robert", "Martin", "Cecil"), "9780132350884");
            _context.Books.Add(new Book(new Money(80m, Currency.RON), bookMetadata3));

            var bookMetadata4 = new BookMetadata("Sapiens: A Brief History of Humankind", new Author("Yuval Noah", "Harari"), "9780062316110");
            _context.Books.Add(new Book(new Money(75m, Currency.RON), bookMetadata4));

            var bookMetadata5 = new BookMetadata("Educated", new Author("Tara", "Westover"), "9780399590504");
            _context.Books.Add(new Book(new Money(44m, Currency.RON), bookMetadata5));
        }

        await _context.SaveChangesAsync();
    }
}
