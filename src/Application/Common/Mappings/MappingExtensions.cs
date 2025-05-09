using AutoMapper;
using AutoMapper.QueryableExtensions;
using KarnelTravel.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Common.Mappings;
public static class MappingExtensions
{
	public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

	public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
		=> queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();


	//paginateion for list
	public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
	this IEnumerable<TDestination> enumerable,
	int pageNumber,
	int pageSize) where TDestination : class
	{
		var result = PaginatedList<TDestination>.CreateFromEnumerable(enumerable, pageNumber, pageSize);
		return Task.FromResult(result);
	}
}
