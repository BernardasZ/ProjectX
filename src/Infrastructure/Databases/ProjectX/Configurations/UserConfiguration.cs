using Domain.Models;
using Infrastructure.Databases.ProjectX.Seeds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class UserConfiguration : ConfigurationBase<UserModel>
{
	public override void Configure(EntityTypeBuilder<UserModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
		builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
		builder.Property(x => x.PassHash).HasMaxLength(255).IsRequired();
		builder.Property(x => x.TokenHash).HasMaxLength(255);
		builder.Property(x => x.FailedLoginCount).IsRequired();

		builder.Navigation(x => x.Role).AutoInclude();
		builder.Navigation(x => x.UserSessions).AutoInclude();

		builder.HasData(UserSeed.GetUserSeed());
	}
}