using Equilobe.Core.Features.Books.Commands;
using Equilobe.Core.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var query = new GetBooksQuery();
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{isbn}/available")]
    public async Task<IActionResult> GetBookStockCount(string isbn)
    {
        var query = new GetAvailableBooksQuery(isbn);
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(AddBookCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        var command = new DeleteBookCommand { BookId = bookId };
        await _mediator.Send(command);
        return Ok();
    }
}
