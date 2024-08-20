using KarnelTravel.Share.Common.Enums;

namespace KarnelTravel.Share.Common.Models;
/// <summary>
/// Base result to be return by the API
/// </summary>
public class AppApiResult<T>
{
    public AppApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Messages = new List<AppMessage>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppApiResult"/> class
    /// </summary>
    public AppApiResult() : this(false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppApiResult"/> class
    /// </summary>
    /// <param name="model">The model</param>
    public AppApiResult(T model) : this()
    {
        Data = model;
    }

    /// <summary>
    /// Indicate if API action is success
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// API Messages
    /// </summary>
    public IList<AppMessage> Messages { get; set; }

    /// <summary>
    /// Data return by API
    /// </summary>
    public T Data { get; set; }

    #region Message

    public void AddMessage(string content, AppMessageType type)
    {
        Messages.Add(new AppMessage { Content = content, Type = type });
    }

    public void AddSuccess(string content)
    {
        Messages.Add(new AppMessage { Content = content, Type = AppMessageType.Success });
    }

    public void AddWarning(string content)
    {
        Messages.Add(new AppMessage { Content = content, Type = AppMessageType.Warning });
    }

    public void AddError(string content)
    {
        Messages.Add(new AppMessage { Content = content, Type = AppMessageType.Error });
    }

    #endregion Message
}

public class AppApiResult : AppApiResult<object>
{
    public AppApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Messages = new List<AppMessage>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppApiResult"/> class
    /// </summary>
    public AppApiResult() : this(false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppApiResult"/> class
    /// </summary>
    /// <param name="model">The model</param>
    public AppApiResult(object model) : this()
    {
        Data = model;
    }
}
