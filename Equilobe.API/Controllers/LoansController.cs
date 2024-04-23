using Equilobe.Core.Features.Loans.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddLoan(LoanBookCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("end")]
    public async Task<IActionResult> EndLoan(ReturnBookCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
