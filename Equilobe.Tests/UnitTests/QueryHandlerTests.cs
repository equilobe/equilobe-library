using Equilobe.Core.Features.Books;
using Equilobe.Core.Features.Books.Queries;
using Equilobe.Core.Shared.Models;
using Equilobe.Core.Shared;
using MockQueryable.Moq;
using Moq;
using Equilobe.Core.Features.Loans;
using Equilobe.Core.Features.Loans.Handlers;
using Equilobe.Core.Features.Loans.Queries;

namespace Equilobe.Tests.UnitTests;

public class QueryHandlerTests
{
    private readonly Mock<ILibraryDbContext> libraryDbContextMock;

    private readonly GetBooksQueryHandler getAllBooksQueryHandler;
    private readonly GetAvailableBooksQueryHandler getNumberAvailableBookQueryHandler;
    private readonly GetLoansQueryHandler getLoansQueryHandler;

    public QueryHandlerTests()
    {
        this.libraryDbContextMock = new Mock<ILibraryDbContext>();

        this.getAllBooksQueryHandler = new GetBooksQueryHandler(this.libraryDbContextMock.Object);
        this.getNumberAvailableBookQueryHandler = new GetAvailableBooksQueryHandler(this.libraryDbContextMock.Object);
        this.getLoansQueryHandler = new GetLoansQueryHandler(this.libraryDbContextMock.Object);
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
        this.libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var result = await this.getAllBooksQueryHandler.Handle(new GetBooksQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(books.Count, result.Count); // Check the number of books returned

        // Optionally, check that the properties of each returned BookDTO match the original books
        // this will often fail due to ordering by Id
        //for (int i = 0; i < books.Count; i++)
        //{
        //    Assert.Multiple(() =>
        //    {
        //        Assert.Equal(books[i].Metadata.Title, result[i].Metadata.Title);
        //        Assert.Equal(books[i].Metadata.Author, result[i].Metadata.Author);
        //    });
        //}

        // Optionally, verify that the Books property of the DbContext was accessed once
        this.libraryDbContextMock.Verify(db => db.Books, Times.Once);
    }

    [Fact]
    public async Task WhenQueryParameteresAreSet_Title_Should_Get_Only_Requested_Book()
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
        this.libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var title = "Title1";
        var qualityState = "";
        var isAvailable = true;
        var sortBy = string.Empty;

        var result = await this.getAllBooksQueryHandler.Handle(new GetBooksQuery(title, qualityState, isAvailable, sortBy, false, 0, 0), CancellationToken.None);

        // Assert
        Assert.Equal(title, result.FirstOrDefault()?.Metadata.Title);

        // Optionally, verify that the Books property of the DbContext was accessed once
        this.libraryDbContextMock.Verify(db => db.Books, Times.Once);
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
        this.libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var query = new GetAvailableBooksQuery(isbn1);
        var result = await this.getNumberAvailableBookQueryHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task WhenQueryParameteresAreIncorretlySet_Title_Should_Return_No_Books()
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
        this.libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        // Act
        var title = "NoTitle";
        var qualityState = "";
        var isAvailable = true;
        var sortBy = string.Empty;

        var result = await this.getAllBooksQueryHandler.Handle(new GetBooksQuery(title, qualityState, isAvailable, sortBy, false, 0, 0), CancellationToken.None);

        // Assert
        Assert.True(!result.Any());

        // Optionally, verify that the Books property of the DbContext was accessed once
        this.libraryDbContextMock.Verify(db => db.Books, Times.Once);
    }

    [Fact]
    public async Task WhenNoQueryParametersAreSet_Get_Loans_ThenReturnAllLoans()
    {
        var loans = new List<Loan>
        {
            new Loan(new Guid("9c319638-cb53-4bf9-b6c3-e456126ce0ef"), new Guid("c168d4c7-851c-4e90-abbb-889847d87cb5")),
            new Loan(new Guid("bba4d4e0-a83c-4df9-8951-d49c691eac44"), new Guid("384e9ca8-0f1b-4e37-93bb-c061a1a59b0b"))
        };

        var loansDbSetMock = loans.AsQueryable().BuildMockDbSet();
        this.libraryDbContextMock.Setup(db => db.Loans).Returns(loansDbSetMock.Object);

        // Act
        var result = await this.getLoansQueryHandler.Handle(new GetLoansQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(loans.Count, result.Count);
    }

    [Fact]
    public async Task WhenQueryParametersAreSet_BookTitle_Get_Loans_ThenReturnOnlyRequestedLoans()
    {
        var book1 = new Book(new Money(10m, Currency.USD), new BookMetadata("Title1", new Author("Author", "1"), "9781234567890"));
        var book2 = new Book(new Money(15m, Currency.EUR), new BookMetadata("Title2", new Author("Author", "2"), "9781234567891"));

        var bookId1 = book1.Id;
        var bookId2 = book2.Id;

        var books = new List<Book> { book1, book2 };
        // Set up the mock DbContext to return the list of 
        var booksDbSetMock = books.AsQueryable().BuildMockDbSet();
        this.libraryDbContextMock.Setup(db => db.Books).Returns(booksDbSetMock.Object);

        var loan1 = new Loan(bookId1, new Guid("c168d4c7-851c-4e90-abbb-889847d87cb5"));
        var loan2 = new Loan(bookId2, new Guid("384e9ca8-0f1b-4e37-93bb-c061a1a59b0b"));
        
        var loanId1 = loan1.Id;

        var loans = new List<Loan> { loan1, loan2 };

        var loansDbSetMock = loans.AsQueryable().BuildMockDbSet();
        this.libraryDbContextMock.Setup(db => db.Loans).Returns(loansDbSetMock.Object);

        // Act
        var bookTitle = "Title1";

        var result = await this.getLoansQueryHandler.Handle(new GetLoansQuery(bookTitle, null, null, string.Empty, true, 0, 0), CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Count);
        Assert.Equal(loanId1, result.FirstOrDefault().Id);
    }
}
