using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.IO;

namespace Flashcards.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<WordEntity>
    {
        public void Configure(EntityTypeBuilder<WordEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Set)
                .WithMany(c => c.Words)
                .HasForeignKey(c => c.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.Property(c => c.Name)
                .IsRequired().HasMaxLength(100);

            builder.Property(c => c.Definition)
                .IsRequired().HasMaxLength(777);

            builder.Property(c => c.ImagePath)
                .HasMaxLength(777);

            builder.Property(c => c.IsFavorite)
                .HasDefaultValue(false);

            builder.Property(c => c.IsLastWord)
                .HasDefaultValue(false);

            builder.HasData(
                new WordEntity { Id = 1, Name = "Water", Definition = "Definiton of water", ImagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("..", "..", ".."))) + "\\wwwroot\\Images\\water.jfif", IsFavorite = false, IsLastWord = false, SetId = 1},
                new WordEntity { Id = 2, Name = "Kettle", Definition = "Definiton of a kettle", ImagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("..", "..", ".."))) + "\\wwwroot\\Images\\kettle.webp", IsFavorite = true, IsLastWord = false, SetId = 1 },
                new WordEntity { Id = 3, Name = "Table", Definition = "Definiton of a table", ImagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("..", "..", ".."))) + "\\wwwroot\\Images\\table.jpg", IsFavorite = true, IsLastWord = false, SetId = 1 }
                );
        }
    }
}