using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Address
    {
        private int _addressid;

        private string _addressline1;

        private string _addressline2;

        private string _city;

        private DateTime _modifieddate;

        private string _postalcode;

        private Guid _rowguid;

        private int _stateprovinceid;

        [DataMember]
        public int AddressID
        {
            get => _addressid;
            set => _addressid = value;
        }

        [DataMember]
        public string AddressLine1
        {
            get => _addressline1;
            set => _addressline1 = value;
        }

        [DataMember]
        public string AddressLine2
        {
            get => _addressline2;
            set => _addressline2 = value;
        }

        [DataMember]
        public string City
        {
            get => _city;
            set => _city = value;
        }

        [DataMember]
        public int StateProvinceID
        {
            get => _stateprovinceid;
            set => _stateprovinceid = value;
        }

        [DataMember]
        public string PostalCode
        {
            get => _postalcode;
            set => _postalcode = value;
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