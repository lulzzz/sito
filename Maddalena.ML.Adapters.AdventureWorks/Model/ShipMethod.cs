using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ShipMethod
    {
        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;

        private decimal _shipbase;
        private int _shipmethodid;

        private decimal _shiprate;

        [DataMember]
        public int ShipMethodID
        {
            get => _shipmethodid;
            set => _shipmethodid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public decimal ShipBase
        {
            get => _shipbase;
            set => _shipbase = value;
        }

        [DataMember]
        public decimal ShipRate
        {
            get => _shiprate;
            set => _shiprate = value;
        }

        [DataMember]
        public Guid rowguid
        {
            get => _rowguid;
            set => _rowguid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}