using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    //DbSet<ProductCategory> ProductCategories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
