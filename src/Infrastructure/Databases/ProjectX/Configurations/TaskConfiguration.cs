using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class TaskConfiguration : ConfigurationBase<TaskModel>
{
	public override void Configure(EntityTypeBuilder<TaskModel> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
		builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
		builder.Property(x => x.Status).IsRequired();

		builder.Navigation(x => x.User).AutoInclude();
	}
}