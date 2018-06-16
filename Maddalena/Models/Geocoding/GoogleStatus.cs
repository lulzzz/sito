namespace Maddalena.Models.Geocoding
{
    public enum GoogleStatus
    {
        Error,
        Ok,
        ZeroResults,
        OverQueryLimit,
        RequestDenied,
        InvalidRequest
    }
}