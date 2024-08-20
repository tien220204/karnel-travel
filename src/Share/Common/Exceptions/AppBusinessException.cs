namespace KarnelTravel.Share.Common.Exceptions;
/// <summary>
/// Business exception which will be catched on API and return the message to user
/// </summary>
/// <seealso cref="System.Exception" />
public class AppBusinessException : Exception
{
    /// <summary>
    /// The biz error use for general business error (un-manage error code in DB)
    /// </summary>
    public const string BizError = "BizError";

    /// <summary>
    /// Gets the message code
    /// </summary>
    /// <value>
    /// The message code
    /// </value>
    public string MessageCode { get; }

    /// <summary>
    /// Gets or sets error data
    /// </summary>
    /// <value>
    /// Error data.
    /// </value>
    public object ErrorData { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBusinessException" /> class
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="code">The code (E00999 by default)</param>
    public AppBusinessException(string message, string code = BizError) : base(message)
    {
        this.MessageCode = code;
    }
}
