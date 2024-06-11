﻿using Equilobe.Core.Features.Books.DTO;
using Equilobe.Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.Core.Features.Books.Commands;

public class DeleteBookCommand : IRequest<BookDTO>
{
    public required Guid BookId { get; init; }
}

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, BookDTO>
{
    private readonly ILibraryDbContext dbContext;

    public DeleteBookCommandHandler(ILibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<BookDTO> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken)
            ?? throw new KeyNotFoundException(nameof(Book));
        this.dbContext.Books.Remove(book);
        return new BookDTO(book);
    }
}
