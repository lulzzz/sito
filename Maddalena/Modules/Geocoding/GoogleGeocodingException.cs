using System;

namespace Maddalena.Modules.Geocoding
{
	public class GoogleGeocodingException : Exception
	{
		const string defaultMessage = "There was an error processing the geocoding request. See Status or InnerException for more information.";

		public GoogleStatus Status { get; private set; }

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
	}
}