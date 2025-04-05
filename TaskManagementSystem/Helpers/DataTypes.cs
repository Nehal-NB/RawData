using System.Data;

public class RetDBAction
{
    private int _mRetValue;
    private bool _mStatus;
    private string _mErDesc;

    public int RetValue
    {
        get { return _mRetValue; }
        set { _mRetValue = value; }
    }

    public bool Status
    {
        get { return _mStatus; }
        set { _mStatus = value; }
    }

    public string ErrorDescription
    {
        get { return _mErDesc; }
        set { _mErDesc = value; }
    }
}

public class RetDataSet
{
    public bool Status { get; set; }
    public string ErrorDescription { get; set; }
    public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();
}

