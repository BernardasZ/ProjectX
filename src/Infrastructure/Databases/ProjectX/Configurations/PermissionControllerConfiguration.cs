using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class PermissionControllerConfiguration : ConfigurationBase<PermissionControllerModel>
{
	public override void Configure(EntityTypeBuilder<PermissionControllerModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
	}
}