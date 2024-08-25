using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KarnelTravel.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
	private readonly IUser _user;
	private readonly TimeProvider _dateTime;

	public AuditableEntityInterceptor(
		IUser user,
		TimeProvider dateTime)
	{
		_user = user;
		_dateTime = dateTime;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public void UpdateEntities(DbContext context)
	{
		if (context == null) return;
		var entries = context.ChangeTracker.Entries().Where(e => e.Entity is IAuditableEntity);

		foreach (var entry in entries)
		{
			var entity = entry.Entity as IAuditableEntity;

			if (entity != null)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entity.CreatedBy = _user.Id;
						entity.Created = DateTimeOffset.UtcNow;
						entity.LastModifiedBy = _user.Id;
						entity.LastModified = DateTimeOffset.UtcNow;
						break;

					case EntityState.Modified:
						entity.LastModifiedBy = _user.Id;
						entity.LastModified = DateTimeOffset.UtcNow;
						break;
				}
			}
		}
	}
}

public static class Extensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(r =>
			r.TargetEntry != null &&
			r.TargetEntry.Metadata.IsOwned() &&
			(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}