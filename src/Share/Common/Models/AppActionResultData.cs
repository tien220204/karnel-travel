namespace KarnelTravel.Share.Common.Models;
public class AppActionResultData<TData, TDetail> where TDetail : class
{
    public bool IsSuccess { get; set; }

    public TData Data { get; set; }

    public TDetail Detail { get; set; }

    public AppActionResultData()
    {
        IsSuccess = false;
        Data = default;
        Detail = default;
    }

    public AppActionResultData(TData data)
    {
        BuildResult(data);
    }

    public AppActionResultData<TData, TDetail> BuildResult(TData data, TDetail detail = null)
    {
        SetInfo(true, detail);
        Data = data;
        return this;
    }

    public AppActionResultData<TData, TDetail> BuildError(TDetail error)
    {
        SetInfo(false, error);
        return this;
    }

    public AppActionResultData<TData, TDetail> SetInfo(bool success, TDetail detail = null)
    {
        IsSuccess = success;
        Detail = detail;
        return this;
    }
}

public class AppActionResultData<TData> : AppActionResultData<TData, string>
{
    public AppActionResultData() : base()
    {
    }

    public AppActionResultData(TData data) : base(data)
    {
    }

    public new AppActionResultData<TData> BuildResult(TData data, string detail = null)
    {
        SetInfo(true, detail);
        Data = data;
        return this;
    }

    public new AppActionResultData<TData> BuildError(string error)
    {
        SetInfo(false, error);
        return this;
    }
}
