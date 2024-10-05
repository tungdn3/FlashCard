using FlashCard.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashCard.Infrastructure.EF.EntityConfigurations;

internal class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Card");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Word)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Meaning)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Example)
            .HasMaxLength(500);

        builder.Property(x => x.Example)
            .HasMaxLength(2048);
    }
}
