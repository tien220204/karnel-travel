using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using MediatR;

namespace KarnelTravel.Application.Features.Hotels.Queries;
public class GetHotelWithFilterAndPaginationQuery : IRequest<AppActionResultData<Common.Models.PaginatedList<HotelDto>>>
{
	public string SearchText { get; set; }
	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}*
