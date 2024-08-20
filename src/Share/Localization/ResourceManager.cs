using System.Globalization;

namespace KarnelTravel.Share.Localization;
public static class ResourceManager
{
    private static readonly System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager(typeof(Resources));

    public static string GetString(string key, CultureInfo culture)
    {
        var resource = _resourceManager.GetString(key);
        if (resource is null)
        {
            resource = "Không tìm thấy data";
        }

        //var resourceSet = _resourceManager.GetResourceSet(culture, true, true);
        //if (resourceSet == null)
        //{
        //    throw new ArgumentException($"Resource set for culture '{culture.Name}' not found.");
        //}

        //var resource = resourceSet.GetString(key);
        //if (resource == null)
        //{
        //    throw new ArgumentException($"Resource '{key}' for culture '{culture.Name}' not found.");
        //}

        return resource;
    }
}


