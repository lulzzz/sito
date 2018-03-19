using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Customer
    {
        private string _accountnumber;
        private int _customerid;

        private DateTime _modifieddate;

        private int _personid;

        private Guid _rowguid;

        private int _storeid;

        private int _territoryid;

        [DataMember]
        public int CustomerID
        {
            get => _customerid;
            set => _customerid = value;
        }

        [DataMember]
        public int PersonID
        {
            get => _personid;
            set => _personid = value;
        }

        [DataMember]
        public int StoreID
        {
            get => _storeid;
            set => _storeid = value;
        }

        [DataMember]
        public int TerritoryID
        {
            get => _territoryid;
            set => _territoryid = value;
        }

        [DataMember]
        public string AccountNumber
        {
            get => _accountnumber;
            set => _accountnumber = value;
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