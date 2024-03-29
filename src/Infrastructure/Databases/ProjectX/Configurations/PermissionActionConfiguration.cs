﻿using Domain.Models;
using Infrastructure.Databases.ProjectX.Seeds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class PermissionActionConfiguration : ConfigurationBase<PermissionActionModel>
{
	public override void Configure(EntityTypeBuilder<PermissionActionModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

		builder.HasData(PermissionActionSeed.GetPermissionActionSeed());
	}
}