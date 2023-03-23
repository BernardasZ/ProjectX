using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class PermissionMappingConfiguration : ConfigurationBase<PermissionMappingModel>
{
	public override void Configure(EntityTypeBuilder<PermissionMappingModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.AllowAllActions).IsRequired();

		builder.Navigation(x => x.Role).AutoInclude();
		builder.Navigation(x => x.Action).AutoInclude();
		builder.Navigation(x => x.Controller).AutoInclude();
	}
}