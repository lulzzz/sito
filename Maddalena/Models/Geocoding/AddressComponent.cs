﻿using System;

namespace Maddalena.Models.Geocoding
{
    public class AddressComponent
    {
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

        public AddressType[] Types { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Types[0], LongName);
        }
    }
}