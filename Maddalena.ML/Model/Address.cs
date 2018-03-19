using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Model
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Address
    {
        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int StateProvince { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}