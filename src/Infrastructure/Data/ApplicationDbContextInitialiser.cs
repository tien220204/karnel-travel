﻿using Duende.IdentityServer.EntityFramework.DbContexts;
using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;

namespace KarnelTravel.Infrastructure.Data;

public static class InitialiserExtensions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

		await initialiser.InitialiseAsync();
		//await initialiser.InitialisePersistedGrantDbAsync();
		// await initialiser.SeedAsync();
	}
}


public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	//Transform into  keycloak
	//private readonly PersistedGrantDbContext _persistedGrantDbContext;
	//private readonly UserManager<ApplicationUser> _userManager;

	public ApplicationDbContextInitialiser(
		ILogger<ApplicationDbContextInitialiser> logger,
		ApplicationDbContext context
		//PersistedGrantDbContext persistedGrantDbContext,
		//UserManager<ApplicationUser> userManager)
		)
	{
		_logger = logger;
		_context = context;
		//_persistedGrantDbContext = persistedGrantDbContext;
		//_userManager = userManager;
	}

	public async Task InitialiseAsync()
	{
		try
		{
			await _context.Database.MigrateAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initialising the database.");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database.");
			throw;
		}
	}

	public async Task InitialisePersistedGrantDbAsync()
	{
		//try
		//{
		//	if ((await _persistedGrantDbContext.Database.GetPendingMigrationsAsync()).Any())
		//	{
		//		await _persistedGrantDbContext.Database.MigrateAsync();
		//	}
		//}
		//catch (Exception ex)
		//{
		//	_logger.LogError(ex, "An error occurred while initialising the database.");
		//	throw;
		//}
	}

	public async Task TrySeedAsync()
	{
		// Default roles
		//var administratorRole = new ApplicationRole(Roles.Administrator);

		//if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
		//{
		//    await _roleManager.CreateAsync(administratorRole);
		//}

		//// Default users
		//var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

		//if (_userManager.Users.All(u => u.UserName != administrator.UserName))
		//{
		//    await _userManager.CreateAsync(administrator, "Administrator1!");
		//    if (!string.IsNullOrWhiteSpace(administratorRole.Name))
		//    {
		//        await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
		//    }
		//}
	}
}