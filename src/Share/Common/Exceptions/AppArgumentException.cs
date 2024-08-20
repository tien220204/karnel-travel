namespace KarnelTravel.Share.Common.Exceptions;

/// <summary>
/// Indicate the argument is invalid
/// </summary>
/// <seealso cref="System.ArgumentException" />
public sealed class AppArgumentException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppArgumentException"/> class
    /// </summary>
    /// <param name="argumentInfo">argument name / message</param>
    /// <param name="usingDefaultMessage">indicate if using default message</param>
    public AppArgumentException(string argumentInfo, bool usingDefaultMessage = true)
        : base(usingDefaultMessage ? $"The parameter {argumentInfo} value is invalid" : argumentInfo)
    {
    }
}
