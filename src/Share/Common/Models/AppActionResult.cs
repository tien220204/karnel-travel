namespace KarnelTravel.Share.Common.Models;
public class AppActionResult
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    public AppActionResult()
    {
        IsSuccess = false;
        Message = null;
    }

    public AppActionResult(bool success, string message = null)
    {
        SetInfo(success, message);
    }

    public AppActionResult BuildResult(string infoMessage)
    {
        IsSuccess = true;
        Message = infoMessage;
        return this;
    }

    public AppActionResult BuildError(string errorMessage)
    {
        IsSuccess = false;
        Message = errorMessage;
        return this;
    }

    public AppActionResult SetInfo(bool success, string message = null)
    {
        IsSuccess = success;
        Message = message;
        return this;
    }
}
