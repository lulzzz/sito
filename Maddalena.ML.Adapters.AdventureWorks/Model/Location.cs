using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Location
    {
        private decimal _availability;

        private decimal _costrate;
        private short _locationid;

        private DateTime _modifieddate;

        private string _name;

        [DataMember]
        public short LocationID
        {
            get => _locationid;
            set => _locationid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public decimal CostRate
        {
            get => _costrate;
            set => _costrate = value;
        }

        [DataMember]
        public decimal Availability
        {
            get => _availability;
            set => _availability = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}