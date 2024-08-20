using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KarnelTravel.Share.Localization;
public static class DependencyInjection
{
    public static IServiceCollection AddLocalizationSupport(this IServiceCollection services, string defaultCulture = "en")
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("vi")
                };

            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }

    public static IApplicationBuilder UseLocalizationSupport(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

        if (options == null || options.Value == null)
        {
            var defaultCulture = "en";
            var culture = new CultureInfo(defaultCulture);

            app.UseRequestLocalization(opts =>
            {
                opts.DefaultRequestCulture = new RequestCulture(culture);
                opts.SupportedCultures = new List<CultureInfo> { culture };
                opts.SupportedUICultures = new List<CultureInfo> { culture };
            });
        }
        else
        {
            app.UseRequestLocalization(options.Value);
        }

        return app;
    }
}
