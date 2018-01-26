using System;

namespace Maddalena.Modules.Geocoding
{
	public class AddressComponent
	{
		public AddressType[] Types { get; set; }
		public string LongName { get; set; }
		public string ShortName { get; set; }

		public AddressComponent(AddressType[] types, string longName, string shortName)
		{
			if (types == null)
				throw new ArgumentNullException(nameof(types));

			if (types.Length < 1)
				throw new ArgumentException("Value cannot be empty.", nameof(types));

			Types = types;
			LongName = longName;
			ShortName = shortName;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Types[0], LongName);
		}
	}
}