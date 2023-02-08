using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Core.Models;

namespace Server.Persistence.Configurations;

public class SubjectsConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(18);
    }
}