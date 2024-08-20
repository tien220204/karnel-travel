
namespace KarnelTravel.Share.Common.Helpers;
public static class ExcelHelper
{
    public static List<T> ConvertDataFromFile<T>(List<List<string>> dataSource) where T : new()
    {
        var result = new List<T>();
        foreach (var item in dataSource)
        {
            var record = new T();
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                if (i <= item.Count - 1)
                {
                    var value = Convert.ChangeType(item[i], propertyInfo.PropertyType);
                    propertyInfo.SetValue(record, value);
                }
            }
            result.Add(record);
        }
        return result;
    }
}
