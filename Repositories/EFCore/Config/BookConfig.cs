using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "The Great Gatsby", Price = 7.99m },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Price = 6.99m },
                new Book { Id = 3, Title = "1984", Price = 9.99m }
            );
        }
    }
}
