using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class BusinessEntityAddress
    {
        private int _addressid;

        private int _addresstypeid;
        private int _businessentityid;

        private DateTime _modifieddate;

        private Guid _rowguid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public int AddressID
        {
            get => _addressid;
            set => _addressid = value;
        }

        [DataMember]
        public int AddressTypeID
        {
            get => _addresstypeid;
            set => _addresstypeid = value;
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