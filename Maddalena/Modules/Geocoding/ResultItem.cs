using System;
using System.Collections.Generic;

namespace Maddalena.Modules.Geocoding
{
	public class ResultItem
	{
		Address input;
		/// <summary>
		/// Original input for this response
		/// </summary>
		public Address Request
		{
			get => input;
		    set
			{
                input = value ?? throw new ArgumentNullException("Input");
			}
		}

		IEnumerable<Address> output;
		/// <summary>
		/// Output for the given input
		/// </summary>
		public IEnumerable<Address> Response
		{
			get => output;
		    set
			{
                output = value ?? throw new ArgumentNullException("Response");
			}
		}

		public ResultItem(Address request, IEnumerable<Address> response)
		{
			Request = request;
			Response = response;
		}
	}
}
