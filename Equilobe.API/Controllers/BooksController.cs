namespace Equilobe.API.Controllers
{
    using Equilobe.Core.Features.Books.Commands;
    using Equilobe.Core.Features.Books.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediator;

        public BooksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(
            [FromQuery] string? title,
            [FromQuery] string? qualityState,
            [FromQuery] bool? isAvailable,
            [FromQuery] string sortBy = "id",
            [FromQuery] bool sortAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageSize > 50)
            {
                pageSize = 50;
            }

            var query = new GetBooksQuery(title, qualityState, isAvailable, sortBy, sortAscending, pageNumber, pageSize);
            var books = await this.mediator.Send(query);
            return Ok(books);
        }

        [HttpGet("{isbn}/available")]
        public async Task<IActionResult> GetBookStockCount(string isbn)
        {
            var query = new GetAvailableBooksQuery(isbn);
            var books = await this.mediator.Send(query);
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(Guid bookId)
        {
            var command = new DeleteBookCommand { BookId = bookId };
            await this.mediator.Send(command);
            return Ok();
        }
    }
}