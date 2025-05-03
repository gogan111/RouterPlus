namespace RouterPlus.Dtos.Responses;

public class OperationResponse
{
    public string? Result { get; set; }

    public ResponseStatus Status
    {
        get
        {
            switch (Result)
            {
                case "0":
                    return ResponseStatus.SUCCESS;
                case "success":
                    return ResponseStatus.SUCCESS;
                default:
                    return ResponseStatus.FAIL;
            }
        }
    }

    public enum ResponseStatus
    {
        SUCCESS,
        FAIL
    }
}