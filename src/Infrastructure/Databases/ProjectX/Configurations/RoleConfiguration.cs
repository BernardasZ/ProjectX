using Domain.Models;
using Infrastructure.Databases.ProjectX.Seeds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class RoleConfiguration : ConfigurationBase<RoleModel>
{
	public override void Configure(EntityTypeBuilder<RoleModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
		builder.Property(x => x.Value).IsRequired();

		builder.HasData(RoleSeed.GetRoleSeed());
	}
}