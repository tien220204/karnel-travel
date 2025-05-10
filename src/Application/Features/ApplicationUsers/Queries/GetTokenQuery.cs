using AutoMapper;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Cache.Contanst;
using KarnelTravel.Share.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiggyCreatures.Caching.Fusion;
using KarnelTravel.Application.Features.ApplicationUsers.Models.Dtos.Token;
using System.Text.Json;

namespace KarnelTravel.Application.Features.ApplicationUsers.Queries;



public record GetTokenQuery : IRequest<AppActionResultData<TokenResponseDto>>
{
	public string Username { get; set; }
	public string Password { get; set; }
}

public class GetTokenQueryHandler : BaseHandler, IRequestHandler<GetTokenQuery, AppActionResultData<TokenResponseDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	private readonly IMapper _mapper;
	private readonly IKeycloakService _keycloakService;

	public GetTokenQueryHandler(
		IApplicationDbContext context,
		IFusionCache fusionCache,
		IMapper mapper,
		IKeycloakService keycloakService)
	{
		_context = context;
		_fusionCache = fusionCache;
		_mapper = mapper;
		_keycloakService = keycloakService;
	}

	public async Task<AppActionResultData<TokenResponseDto>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<TokenResponseDto>();

		var tokenResponse = await _keycloakService.GetUserToken(request.Username, request.Password);

		var obj = JsonSerializer.Deserialize<TokenResponseDto>(tokenResponse, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		return BuildMultilingualResult(result, obj, Resources.INF_MSG_SUCCESSFULLY);
	}

}