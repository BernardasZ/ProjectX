using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class UserSessionConfiguration : ConfigurationBase<UserSessionModel>
{
	public override void Configure(EntityTypeBuilder<UserSessionModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.SessionIdentifier).HasMaxLength(255).IsRequired();
		builder.Property(x => x.CreateDt).ValueGeneratedOnAdd().IsRequired();
		builder.Property(x => x.Ip).HasMaxLength(15).IsRequired();
	}
}