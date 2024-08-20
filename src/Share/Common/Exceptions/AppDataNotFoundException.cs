using System.Data;

namespace KarnelTravel.Share.Common.Exceptions;

/// <summary>
/// Indicate that the data is not found
/// </summary>
/// <seealso cref="System.Data.DataException" />
public class AppDataNotFoundException : DataException
{
    /// <summary>
    /// Indicate that the data is not found
    /// </summary>
    /// <param name="data">The data</param>
    public AppDataNotFoundException(string data) : base(data)
    {
    }

    /// <summary>
    /// Indicate that the data is not found
    /// </summary>
    /// <param name="objectName">Name of the object (table)</param>
    /// <param name="filter">The condition to find the object</param>
    public AppDataNotFoundException(string objectName, string filter) : base($"{objectName}: {filter}")
    {
    }
}
