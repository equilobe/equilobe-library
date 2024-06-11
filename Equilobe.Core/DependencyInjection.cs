﻿using Equilobe.Core.Features.Books.Commands;
using Equilobe.Core.Features.Books.Queries;
using Equilobe.Core.Features.Loans.Commands;
using Equilobe.Core.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Equilobe.Core.Features.Loans.Handlers;
using Equilobe.Core.Features.Loans.Interfaces;
using Equilobe.Core.Features.Loans.Services;

namespace Equilobe.Core;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(BookLoanedEventHandler)));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(BookLoanedEvent)));

        services.AddTransient<GetBooksQueryHandler>();
        services.AddTransient<AddBookCommandHandler>();
        services.AddTransient<DeleteBookCommandHandler>();
        services.AddTransient<GetAvailableBooksQueryHandler>();
        services.AddTransient<GetLoansQueryHandler>();
        services.AddTransient<LoanBookCommandHandler>();
        services.AddTransient<ReturnBookCommandHandler>();

        services.AddTransient<BookLoanedEventHandler>();
        services.AddTransient<BookReturnedEventHandler>();

        services.AddScoped<IPenaltyCalculator, PenaltyCalculator>();
    }
}
