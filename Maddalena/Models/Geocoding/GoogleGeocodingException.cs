using System;

namespace Maddalena.Models.Geocoding
{
    public class GoogleGeocodingException : Exception
    {
        const string defaultMessage =
            "There was an error processing the geocoding request. See Status or InnerException for more information.";

        public GoogleGeocodingException(GoogleStatus status)
            : base(defaultMessage)
        {
            Status = status;
        }

        public GoogleGeocodingException(Exception innerException)
            : base(defaultMessage, innerException)
        {
            Status = GoogleStatus.Error;
        }

        public GoogleStatus Status { get; private set; }
    }
}