using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using PromptTaskApi.Infrastructure.Data;

#nullable disable

namespace PromptTaskApi.Infrastructure.Migrations;

[DbContext(typeof(PromptTaskDbContext))]
partial class PromptTaskDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("PromptTaskApi.Domain.Prompts.PromptJob", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<int>("AttemptCount")
                .HasColumnType("int");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("datetimeoffset");

            b.Property<string>("ErrorMessage")
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            b.Property<DateTimeOffset?>("ProcessingFinishedAtUtc")
                .HasColumnType("datetimeoffset");

            b.Property<DateTimeOffset?>("ProcessingStartedAtUtc")
                .HasColumnType("datetimeoffset");

            b.Property<string>("Prompt")
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnType("nvarchar(4000)");

            b.Property<string>("Result")
                .HasMaxLength(16000)
                .HasColumnType("nvarchar(max)");

            b.Property<byte[]>("RowVersion")
                .IsConcurrencyToken()
                .IsRequired()
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("rowversion");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("datetimeoffset");

            b.HasKey("Id");

            b.HasIndex("Status", "CreatedAtUtc");

            b.ToTable("PromptJobs", (string)null);
        });
#pragma warning restore 612, 618
    }
}
