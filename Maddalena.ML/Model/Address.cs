using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Mongolino;

namespace Maddalena.ML.Model
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Address : DBObject<Address>
    {
        public ObjectRef<Person> Person { get; set; }

        public AddressType Type { get; set; }

        [DataMember] public string AddressLine1 { get; set; }

        [DataMember] public string AddressLine2 { get; set; }

        [DataMember] public string City { get; set; }

        [DataMember] public string StateProvince { get; set; }

        [DataMember] public string Country { get; set; }

        [DataMember] public string PostalCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public string Company { get; set; }
    }
}