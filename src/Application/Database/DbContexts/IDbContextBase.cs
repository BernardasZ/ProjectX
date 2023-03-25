using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Application.Database.DbContexts;

public interface IDbContextBase
{
	int SaveChanges();

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors
		| DynamicallyAccessedMemberTypes.NonPublicConstructors
		| DynamicallyAccessedMemberTypes.PublicFields
		| DynamicallyAccessedMemberTypes.NonPublicFields
		| DynamicallyAccessedMemberTypes.PublicProperties
		| DynamicallyAccessedMemberTypes.NonPublicProperties
		| DynamicallyAccessedMemberTypes.Interfaces)] TEntity>()
		where TEntity : class;
}