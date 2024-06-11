using Equilobe.Core.Features.Loans.Commands;
using Equilobe.Core.Features.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IMediator mediator;

    public LoansController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoans(
        [FromQuery] string? bookTitle,
        [FromQuery] Guid? bookId,
        [FromQuery] Guid? userId,
        [FromQuery] string sortBy = "userId",
        [FromQuery] bool sortAscending = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
        )
    {
        if (pageSize > 50)
        {
            pageSize = 50;
        }

        var query = new GetLoansQuery(bookTitle, bookId, userId, sortBy, sortAscending, pageNumber, pageSize);
        var loans = await this.mediator.Send(query);
        return Ok(loans);
    }

    [HttpPost]
    public async Task<IActionResult> AddLoan(LoanBookCommand command)
    {
        await this.mediator.Send(command);
        return Ok();
    }

    [HttpPost("end")]
    public async Task<IActionResult> EndLoan(ReturnBookCommand command)
    {
        await this.mediator.Send(command);
        return Ok();
    }
}
