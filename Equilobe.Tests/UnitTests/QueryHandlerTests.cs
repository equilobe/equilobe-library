using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Books.Queries;
using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared;
using MockQueryable.Moq;
using Moq;

namespace Equilobe.Tests.UnitTests;

public class QueryHandlerTests
{
    private readonly Mock<ILibraryDbContext> _libraryDbContextMock;

    private readonly GetBooksQueryHandler _getAllBooksQueryHandler;
    private readonly GetAvailableBooksQueryHandler _getNumberAvailableBookQueryHandler;

    public QueryHandlerTests()
    {
        _libraryDbContextMock = new Mock<ILibraryDbContext>();

        _getAllBooksQueryHandler = new GetBooksQueryHandler(_libraryDbContextMock.Object);
        _getNumberAvailableBookQueryHandler = new GetAvailableBooksQueryHandler(_libraryDbContextMock.Object);
    }

    [Fact]
    public async Task Should_Get_All_Books()
    {
        // Define a list of books
        var books = new List<Book>
            {
                new Book(new Money(10m, Currency.USD), new BookMetadata("Title1", new Author("Author","1"), "9781234567890")),
                new Book(new Money(15m, Currency.EUR), new BookMetadata("Title2", new Author("Author","2"), "9781234567891")),
                // Add more books as needed
            };

        // Set up the mock DbContext to return the list of 
        var booksDbSetMock = books.AsQueryable().BuildMockDbSet();
        _libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var result = await _getAllBooksQueryHandler.Handle(new GetBooksQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(books.Count, result.Count); // Check the number of books returned

        // Optionally, check that the properties of each returned BookDTO match the original books
        for (int i = 0; i < books.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.Equal(books[i].Metadata.Title, result[i].Metadata.Title);
                Assert.Equal(books[i].Metadata.Author, result[i].Metadata.Author);
            });
        }

        // Optionally, verify that the Books property of the DbContext was accessed once
        _libraryDbContextMock.Verify(db => db.Books, Times.Once);
    }

    [Fact]
    public async Task Should_Get_Number_Available_Books()
    {
        var isbn1 = "9781234567890";
        var bookMetadata = new BookMetadata("Title1", new Author("Author", "1"), isbn1);
        var book1 = new Book(new Money(10m, Currency.USD), bookMetadata);
        var book2 = new Book(new Money(9m, Currency.EUR), bookMetadata);

        // Set up the mock DbContext to return the list of 
        var booksDbSetMock = new List<Book>() { book1, book2 }
            .AsQueryable()
            .BuildMockDbSet();
        _libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var query = new GetAvailableBooksQuery(isbn1);
        var result = await _getNumberAvailableBookQueryHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result);
    }
}
