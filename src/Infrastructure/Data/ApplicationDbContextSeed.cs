using Microsoft.Extensions.DependencyInjection;

namespace KarnelTravel.Infrastructure.Data;
public class ApplicationDbContextSeed
{

	//public static async Task SeedProductUnitAsync(IServiceProvider serviceProvider)
	//{
	//	using var scope = serviceProvider.CreateScope();
	//	var services = scope.ServiceProvider;

	//	try
	//	{
	//		var context = services.GetRequiredService<ApplicationDbContext>();
	//		if (!context.ProductUnits.Any())
	//		{
	//			var productUnits = new List<ProductUnit>() {
	//								new ProductUnit { Name = "cái", IsBaseUnit=true },
	//			};

	//			await context.ProductUnits.AddRangeAsync(productUnits);
	//			await context.SaveChangesAsync();
	//		}
	//	}
	//	catch (Exception ex)
	//	{
	//		//TODO
	//	}
	//}
}
