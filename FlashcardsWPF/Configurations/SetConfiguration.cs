using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashcards.Configurations
{
    public class SetConfiguration : IEntityTypeConfiguration<SetEntity>
    {
        public void Configure(EntityTypeBuilder<SetEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Words)
                .WithOne(o => o.Set);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.Property(c => c.Name)
                .IsRequired().HasMaxLength(20);

            builder.HasData(new SetEntity { Id = 1, Name = "1" });
        }
    }
}