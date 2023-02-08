using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Core.Models;

namespace Server.Persistence.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Username)
            .HasMaxLength(18);

        builder.HasIndex(u => u.Username)
            .IsUnique();
    }
}