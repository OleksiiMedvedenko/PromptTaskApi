using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Infrastructure.Data.Configurations;

internal sealed class PromptJobConfiguration : IEntityTypeConfiguration<PromptJob>
{
    public void Configure(EntityTypeBuilder<PromptJob> builder)
    {
        builder.ToTable("PromptJobs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Prompt)
            .HasMaxLength(4_000)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Result)
            .HasMaxLength(16_000);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(2_000);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => new { x.Status, x.CreatedAtUtc });
    }
}
