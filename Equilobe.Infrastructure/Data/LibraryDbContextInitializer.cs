using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Loans;
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
    private readonly ILogger<LibraryDbContextInitializer> logger;
    private readonly LibraryDbContext context;

    public LibraryDbContextInitializer(LibraryDbContext context, ILogger<LibraryDbContextInitializer> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occurred while initialising the database.");
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
            this.logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedAsync()
    {
        var userId1 = new Guid();
        var userId2 = new Guid();

        var bookId1 = new Guid();
        var bookId2 = new Guid();
        var bookId3 = new Guid();

        //this.context.Users.RemoveRange(this.context.Users);
        //this.context.Books.RemoveRange(this.context.Books);

        if (!this.context.Users.Any())
        {
            this.context.Users.Add(new User("Bibliotecar", UserRole.Librarian));
            var user1 = new User("Cititor1", UserRole.Reader);
            this.context.Users.Add(user1);
            userId1 = user1.Id;
            var user2 = new User("Cititor2", UserRole.Reader);
            this.context.Users.Add(user2);
            userId2 = user2.Id;
        }

        if (!context.Books.Any())
        {
            var bookMetadata1 = new BookMetadata("Atomic Habits", new Author("James", "Clear"), "9780735211292");
            var book1 = new Book(new Money(70m, Currency.RON), bookMetadata1);
            this.context.Books.Add(book1);
            bookId1 = book1.Id;

            var bookMetadata2 = new BookMetadata("The Pragmatic Programmer", new Author("Andrew", "Hunt"), "9780201616224");
            var book2 = new Book(new Money(50m, Currency.RON), bookMetadata2);
            this.context.Books.Add(book2);
            bookId2 = book2.Id;

            var bookMetadata3 = new BookMetadata("Clean Code", new Author("Robert", "Martin", "Cecil"), "9780132350884");
            var book3 = new Book(new Money(80m, Currency.RON), bookMetadata3);
            this.context.Books.Add(book3);

            var bookMetadata4 = new BookMetadata("Sapiens: A Brief History of Humankind", new Author("Yuval Noah", "Harari"), "9780062316110");
            this.context.Books.Add(new Book(new Money(75m, Currency.RON), bookMetadata4));

            var bookMetadata5 = new BookMetadata("Educated", new Author("Tara", "Westover"), "9780399590504");
            this.context.Books.Add(new Book(new Money(44m, Currency.RON), bookMetadata5));
        }

        if(!this.context.Loans.Any())
        {
            var loan1 = new Loan(userId1, bookId1);
            this.context.Loans.Add(loan1);
            var loan2 = new Loan(userId2, bookId2);
            this.context.Loans.Add(loan2);
            var loan3 = new Loan(userId2, bookId3);
            this.context.Loans.Add(loan3);
        }

        await this.context.SaveChangesAsync();
    }
}
