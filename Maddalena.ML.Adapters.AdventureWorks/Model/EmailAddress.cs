using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class EmailAddress
    {
        private int _businessentityid;

        private string _emailaddress;

        private int _emailaddressid;

        private DateTime _modifieddate;

        private Guid _rowguid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public int EmailAddressID
        {
            get => _emailaddressid;
            set => _emailaddressid = value;
        }

        [DataMember]
        public string Address
        {
            get => _emailaddress;
            set => _emailaddress = value;
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