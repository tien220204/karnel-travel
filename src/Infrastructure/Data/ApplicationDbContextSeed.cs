using KarnelTravel.Domain.Entities.Features.Hotel;
using Microsoft.Extensions.DependencyInjection;

namespace KarnelTravel.Infrastructure.Data;
public class ApplicationDbContextSeed
{

	public static async Task SeedProductUnitAsync(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var services = scope.ServiceProvider;

		try
		{
			var context = services.GetRequiredService<ApplicationDbContext>();
			if (!context.Style.Any())
			{
				var styles = new List<Style>() {
									new Style { Name = "Modern", Description =  "modern feature" },
									new Style { Name = "Classic", Description =  "classic feature" },
									new Style { Name = "Contemporary", Description =  "contemporary feature" },
									new Style { Name = "Traditional", Description =  "traditional feature" },
									new Style { Name = "Rustic", Description =  "rustic feature" },
									new Style { Name = "Industrial", Description =  "industrial feature" },
									new Style { Name = "Minimalist", Description =  "minimalist feature" },
									new Style { Name = "Vintage", Description =  "vintage feature" },
				};

				await context.Style.AddRangeAsync(styles);
				await context.SaveChangesAsync();
			}
		}
		catch (Exception ex)
		{
			//TODO
		}
	}
}
