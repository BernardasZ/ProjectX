using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.ProjectX.Configurations;

internal class ConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
	where TEntity : ModelBase
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
	}
}