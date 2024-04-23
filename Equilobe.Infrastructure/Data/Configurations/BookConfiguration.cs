using Equilobe.Core.Features.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equilobe.Infrastructure.Data.Configurations;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.OwnsOne(b => b.RentPrice);
        builder.OwnsOne(b => b.Metadata, metadata =>
        {
            metadata.ToTable("BooksMetadata");
            metadata.Property(m => m.Title).IsRequired();
            metadata.Property(m => m.ISBN).IsRequired();
            metadata.HasOne(m => m.Author).WithMany().HasForeignKey(m => m.AuthorId);
        });
    }
}
