using Data.Models.AuthenticationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.DateAdded)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.DateExpired)
            .IsRequired()
            .HasColumnType("datetime2");
    }
}
